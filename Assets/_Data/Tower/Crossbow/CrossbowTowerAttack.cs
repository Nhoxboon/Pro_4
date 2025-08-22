using UnityEngine;

public class CrossbowTowerAttack : TowerAttack
{
    [Header("Crossbow Tower Setup")] [SerializeField]
    protected int damage = 2;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
    }

    protected override void Attack()
    {
        base.Attack();
        if (Physics.Raycast(gunPoint.position, towerCtrl.Targeting.DirectionToTarget(gunPoint), out RaycastHit hitInfo,
                Mathf.Infinity, whatIsTargetable))
        {
            towerCtrl.Rotation.TowerHead.forward = towerCtrl.Targeting.DirectionToTarget(gunPoint);

            if (hitInfo.transform.TryGetComponent(out Enemy enemy))
            {
                IDamageable damageable = enemy.Core.DamageReceiver;
                damageable?.TakeDamage(damage);
            }

            CreateAttackVFX(hitInfo);
            AudioManager.Instance.PlaySFX(attackSFX, true);
        }
    }

    protected void CreateAttackVFX(RaycastHit hitInfo)
    {
        if (towerCtrl.Visuals is not CrossbowVisuals visual) return;
        visual.CreateOnHitFX(hitInfo.point);
        visual.PlayAttackVFX(gunPoint.position, hitInfo.point);
        visual.ReloadVFX(attackCooldown);
    }
}