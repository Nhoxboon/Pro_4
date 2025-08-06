using UnityEngine;

public class CrossbowTower : Tower
{
    [Header("Crossbow Tower Setup")] [SerializeField]
    protected int damage = 2;

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
        base.Attack();
        if (Physics.Raycast(gunPoint.position, DirectionToTarget(gunPoint), out RaycastHit hitInfo, Mathf.Infinity,
                whatIsTargetable))
        {
            towerHead.forward = DirectionToTarget(gunPoint);

            if (hitInfo.transform.TryGetComponent(out Enemy enemy))
            {
                IDamageable damageable = enemy.Core.DamageReceiver;
                damageable.TakeDamage(damage);
            }

            visual.CreateOnHitFX(hitInfo.point);
            visual.PlayAttackVFX(gunPoint.position, hitInfo.point);
            visual.ReloadVFX(attackCooldown);
            AudioManager.Instance.PlaySFX(attackSFX, true);
        }
    }
}