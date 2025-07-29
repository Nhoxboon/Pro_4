using UnityEngine;

public class CrossbowTower : Tower
{
    [Header("Crossbow Tower Setup")] [SerializeField]
    protected int damage = 2;

    [SerializeField] protected Transform gunPoint;

    [SerializeField] protected CrossbowVisual visual;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
        LoadCrossbowVisual();
    }

    protected void LoadGunPoint()
    {
        if (gunPoint != null) return;
        gunPoint = towerHead.Find("GunPoint");
        DebugTool.Log(transform.name + " :LoadGunPoint", gameObject);
    }

    protected void LoadCrossbowVisual()
    {
        if (visual != null) return;
        visual = GetComponentInChildren<CrossbowVisual>();
        DebugTool.Log(transform.name + " :LoadCrossbowVisual", gameObject);
    }

    protected override void Attack()
    {
        if (Physics.Raycast(gunPoint.position, DirectionToTarget(gunPoint), out RaycastHit hitInfo, Mathf.Infinity,
                whatIsTargetable))
        {
            towerHead.forward = DirectionToTarget(gunPoint);

            ShieldForEnemy shield;
            IDamageable damageable;
            Enemy enemyTarget = null;

            bool hasShield = hitInfo.collider.TryGetComponentInChildren(out shield);
            bool hasDamageable = hitInfo.transform.TryGetComponentInChildren(out damageable);

            if (hasDamageable && !hasShield)
            {
                damageable.TakeDamage(damage);
                enemyTarget = currentTarget;
            }

            if (hasShield)
                shield.TakeDamage(damage);

            visual.CreateOnHitFX(hitInfo.point);
            visual.PlayAttackVFX(gunPoint.position, hitInfo.point, enemyTarget);
            visual.ReloadVFX(attackCooldown);
            AudioManager.Instance.PlaySFX(attackSFX, true);
        }
    }
}