using System;
using System.Collections;
using UnityEngine;

public class SpiderNestAttack : TowerAttack
{
    [Header("Spider Nest Details")] 
    [SerializeField] protected float damage = 10f;
    [Range(0, 1)] [SerializeField] protected float attackTimeMultiplier = 0.4f;
    [SerializeField] protected float reloadTimeMultiplier = 0.6f;

    protected string spiderBotName = "SpiderNest";
    protected Transform[] activeSpider;
    protected int spiderIndex;
    
    protected Vector3 offset = new Vector3(0, -0.15f, 0);

    protected override void Start()
    {
        base.Start();
        reloadTimeMultiplier = 1 - attackTimeMultiplier;
    }

    private void SetSpiderBotReady()
    {
        if (towerCtrl.Visual is not SpiderNestVisual visual) return;
        activeSpider = new Transform[visual.AttachPoint.Length];
        for (int i = 0; i < activeSpider.Length; i++)
        {
            activeSpider[i] =
                ProjectileSpawner.Instance.Spawn(spiderBotName, visual.AttachPoint[i].position + offset,
                    Quaternion.identity);
            activeSpider[i].SetParent(visual.AttachPoint[i]);
            activeSpider[i].gameObject.SetActive(true);
        }
    }

    protected override bool CanAttack() =>
        Time.time > lastAttackTime + attackCooldown && towerCtrl.Targeting.AtLeastOneTargetInRange();

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(AttackCoroutine());
    }

    protected IEnumerator AttackCoroutine()
    {
        if (towerCtrl.Visual is not SpiderNestVisual visual) yield break;
        Transform currentWeb = visual.WebSet[spiderIndex];
        Transform currentAttachPoint = visual.AttachPoint[spiderIndex];
        float attackTime = (attackCooldown / 4) * attackTimeMultiplier;
        float reloadTime = (attackCooldown / 4) * reloadTimeMultiplier;

        yield return visual.ChangeScaleCoroutine(currentWeb, 1, attackTime);

        activeSpider[spiderIndex].TryGetComponent(out SpiderNest spider);
        spider.SetupSpider(damage);
        activeSpider[spiderIndex] = null;

        yield return visual.ChangeScaleCoroutine(currentWeb, 0.1f, reloadTime);

        activeSpider[spiderIndex] =
            ProjectileSpawner.Instance.Spawn(spiderBotName, currentAttachPoint.position + offset, Quaternion.identity);
        activeSpider[spiderIndex].SetParent(currentAttachPoint);
        activeSpider[spiderIndex].gameObject.SetActive(true);

        spiderIndex = (spiderIndex + 1) % activeSpider.Length;
    }

    public override void ResetAttack()
    {
        base.ResetAttack();
        spiderIndex = 0;
        if (activeSpider != null)
        {
            foreach (var spider in activeSpider)
                if (spider != null)
                    ProjectileSpawner.Instance.Despawn(spider.gameObject);
        }

        SetSpiderBotReady();
    }
}