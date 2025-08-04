using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : CoreComponent
{
    [SerializeField] protected string atkPrefab = "Spider_EMP";
    [SerializeField] protected LayerMask whatIsTower;
    [SerializeField] protected float towerCheckRadius = 4.5f;
    [SerializeField] protected float attackCooldown = 8f;
    [SerializeField] protected float attackEffectDuration = 3f;
    [SerializeField] protected float attackDuration = 5f;
    protected float attackTimer;

    protected void OnEnable() => attackTimer = attackCooldown;

    protected override void Start()
    {
        base.Start();
        attackTimer = attackCooldown;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
            AttemptToAttack();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLayerMask();
    }

    protected void LoadLayerMask()
    {
        if (whatIsTower != 0) return;
        whatIsTower = LayerMask.GetMask("Tower");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    protected void AttemptToAttack()
    {
        Transform target = FindRandomTower();
        if (target is null) return;
        attackTimer = attackCooldown;
        Transform newAttack = ProjectileSpawner.Instance?.Spawn(atkPrefab,
            core.Root.transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);

        newAttack?.gameObject.SetActive(true);

        if (newAttack?.TryGetComponent(out EMPAttack empAttack) == true)
            empAttack.SetUpEMP(attackEffectDuration, target.position, attackDuration);
    }

    protected Transform FindRandomTower()
    {
        Collider[] towers = Physics.OverlapSphere(core.Root.transform.position, towerCheckRadius, whatIsTower);
        // return towers.Length > 0 ? towers[Random.Range(0, towers.Length)].transform : null;

        List<Transform> validTowers = new();
        for (int i = 0; i < towers.Length; i++)
        {
            if(towers[i].TryGetComponent(out Tower tower) && !validTowers.Contains(towers[i].transform))
                validTowers.Add(tower.transform);
        }
        return validTowers.Count > 0 ? validTowers[Random.Range(0, validTowers.Count)] : null;
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(core.Root.transform.position, towerCheckRadius);
}