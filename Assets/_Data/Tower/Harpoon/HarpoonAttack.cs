using System.Collections;
using UnityEngine;

public class HarpoonAttack : TowerAttack
{
    [Header("Harpoon Details")] 
    [SerializeField] protected Transform projectileDefaultPosition;
    [SerializeField] protected float projectileSpeed = 15f;
    [SerializeField] protected GameObject harpoon;

    protected bool reachedTarget;
    protected bool busyWithAttack;
    public bool BusyWithAttack => busyWithAttack;
    protected Harpoon currentProjectile;
    protected Coroutine damageOverTimeCoroutine;

    [Header("Damage Details")]
    [SerializeField] protected float initialDamage = 5f;
    [SerializeField] protected float damageOverTime = 10f;
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
        LoadHarpoon();
    }

    protected void LoadProjectileDefaultPosition()
    {
        if (projectileDefaultPosition != null) return;
        projectileDefaultPosition = towerCtrl.Rotation.TowerHead.Find("tower_harpoon/harpoon");
        DebugTool.Log(transform.name + " :LoadProjectileDefaultPosition", gameObject);
    }

    protected void LoadHarpoon()
    {
        if (harpoon != null) return;
        harpoon = Resources.Load<GameObject>("Projectile/Harpoon");
        DebugTool.Log(transform.name + " :LoadHarpoon", gameObject);
    }

    protected override bool CanAttack() => base.CanAttack() && !busyWithAttack;

    protected override void Attack()
    {
        base.Attack();

        if (!Physics.Raycast(gunPoint.position, gunPoint.forward, out RaycastHit hitInfo, Mathf.Infinity,
                whatIsTargetable)) return;
        if (hitInfo.collider.TryGetComponent(out Enemy enemy))
            towerCtrl.Targeting.SetTarget(enemy);
        busyWithAttack = true;

        currentProjectile.SetupHarpoon(towerCtrl.Targeting.CurrentTarget, projectileSpeed, towerCtrl.transform);
        if(towerCtrl.Visuals is HarpoonVisuals visuals)
            visuals.EnableChainVisuals(true, currentProjectile.GetConnectionPoint());

        AudioManager.Instance.PlaySFX(attackSFX, true);
        StartCoroutine(ResetAttackCoroutine());
    }

    protected void CreateProjectile()
    {
        if(currentProjectile is not null) return;
        GameObject newProjectile = Instantiate(harpoon, projectileDefaultPosition.position,
            projectileDefaultPosition.rotation, towerCtrl.Rotation.TowerHead);
        newProjectile.transform.localScale = Vector3.one;
        newProjectile.transform.localRotation = Quaternion.identity;

        currentProjectile = newProjectile.TryGetComponent(out Harpoon projectile) ? projectile : null;
    }

    public void ActivateAttack()
    {
        Enemy currentEnemy = towerCtrl.Targeting.CurrentTarget;
        if (currentEnemy is null)
        {
            ResetAttack();
            return;
        }

        reachedTarget = true;
        if(currentEnemy.Core.Death is FlyDeath death)
            death.AddObservingTower(towerCtrl.transform);
        if(towerCtrl.Visuals is HarpoonVisuals visuals)
            visuals.CreateElectrifyVFX(currentEnemy.transform);
        currentEnemy.Core.Movement.SlowEnemy(slowEffect, overTimeEffectDuration);
        IDamageable damageable = towerCtrl.Targeting.CurrentTarget.Core.DamageReceiver;
        damageable?.TakeDamage(initialDamage);

        if(damageOverTimeCoroutine != null) StopCoroutine(damageOverTimeCoroutine);
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

    protected IEnumerator ResetAttackCoroutine()
    {
        yield return new WaitForSeconds(1);
        ResetAttackIfMissed();
    }

    protected void ResetAttackIfMissed()
    {
        if (reachedTarget) return;
        ResetAttack();
    }

    public override void ResetAttack()
    {
        lastAttackTime = Time.time;
        if (gunPoint)
            gunPoint.localRotation = Quaternion.identity;

        StopAllCoroutines();
        reachedTarget = false;
        busyWithAttack = false;

        if (towerCtrl.Visuals is HarpoonVisuals visuals)
            visuals.EnableChainVisuals(false);
        currentProjectile?.ResetHarpoon(projectileDefaultPosition.position, projectileDefaultPosition.rotation,
            towerCtrl.Rotation.TowerHead);
    }
}