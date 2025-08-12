using System.Collections;
using UnityEngine;

public class HarpoonAttack : TowerAttack
{
    [Header("Harpoon Details")] 
    [SerializeField] protected Transform projectileDefaultPosition;
    [SerializeField] protected float projectileSpeed = 15f;
    protected string projectile = "Harpoon";

    protected bool busyWithAttack;
    public bool BusyWithAttack => busyWithAttack;
    protected Harpoon currentProjectile;
    protected Coroutine damageOverTimeCoroutine;

    [Header("Damage Details")] 
    [SerializeField] protected float initialDamage = 10f;
    [SerializeField] protected float damageOverTime = 5f;
    [SerializeField] protected float overTimeEffectDuration = 4f;
    [Range(0, 1)] 
    [SerializeField] protected float slowEffect = 0.7f;

    protected override void Start()
    {
        base.Start();
        CreateProjectile();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
        LoadProjectileDefaultPosition();
    }

    protected void LoadProjectileDefaultPosition()
    {
        if (projectileDefaultPosition != null) return;
        projectileDefaultPosition = towerCtrl.Rotation.TowerHead.Find("tower_harpoon/harpoon");
        DebugTool.Log(transform.name + " :LoadProjectileDefaultPosition", gameObject);
    }

    protected override bool CanAttack() => base.CanAttack() && !busyWithAttack;

    protected override void Attack()
    {
        base.Attack();

        if (!Physics.Raycast(gunPoint.position, gunPoint.forward, out RaycastHit hitInfo, Mathf.Infinity,
                whatIsTargetable)) return;
        busyWithAttack = true;
        currentProjectile.SetupHarpoon(towerCtrl.Targeting.CurrentTarget, projectileSpeed, towerCtrl.transform);
    }

    protected void CreateProjectile()
    {
        Transform newProjectile =
            ProjectileSpawner.Instance.Spawn(projectile, projectileDefaultPosition.position, Quaternion.identity);
        newProjectile.gameObject.SetActive(true);
        newProjectile.SetParent(towerCtrl.Rotation.TowerHead);
        newProjectile.localRotation = Quaternion.identity;
        newProjectile.localScale = Vector3.one;

        currentProjectile = newProjectile.TryGetComponent(out Harpoon harpoon) ? harpoon : null;
    }

    public void ActivateAttack()
    {
        if (towerCtrl.Targeting.CurrentTarget is null)
        {
            ResetAttack();
            return;
        }
        towerCtrl.Targeting.CurrentTarget.Core.Movement.SlowEnemy(slowEffect, overTimeEffectDuration);
        IDamageable damageable = towerCtrl.Targeting.CurrentTarget.Core.DamageReceiver;
        damageable?.TakeDamage(initialDamage);
        
        damageOverTimeCoroutine = StartCoroutine(DamageOverTimeCoroutine(damageable));
    }

    protected IEnumerator DamageOverTimeCoroutine(IDamageable damageable)
    {
        float time = 0f;
        float damageFrequency = overTimeEffectDuration / damageOverTime;
        float damagePerTick = damageOverTime / (overTimeEffectDuration / damageFrequency);

        while (time < overTimeEffectDuration)
        {
            damageable?.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(damageFrequency);
            time += damageFrequency;
        }
        ResetAttack();
    }

    public override void ResetAttack()
    {
        base.ResetAttack();
        if (damageOverTimeCoroutine != null) StopCoroutine(damageOverTimeCoroutine);
        busyWithAttack = false;
        if (currentProjectile is null) return;
        ProjectileSpawner.Instance.BackToHolder(currentProjectile.gameObject);
        ProjectileSpawner.Instance.Despawn(currentProjectile.gameObject);
        currentProjectile = null;
        CreateProjectile();
    }
}