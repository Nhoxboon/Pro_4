using UnityEngine;

public class CrossbowTowerAttack : TowerAttack
{
    [Header("Crossbow Tower Setup")]
    [SerializeField] protected int damage = 2;
    [SerializeField] protected CrossbowVisual visual;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
        LoadCrossbowVisual();
    }

    protected void LoadCrossbowVisual()
    {
        if (visual != null) return;
        visual = transform.parent.GetComponentInChildren<CrossbowVisual>();
        DebugTool.Log(transform.name + " :LoadCrossbowVisual", gameObject);
    }

    public override void Attack()
    {
        base.Attack();
        if (Physics.Raycast(gunPoint.position, towerCtrl.Targeting.DirectionToTarget(gunPoint), out RaycastHit hitInfo,
                Mathf.Infinity, whatIsTargetable))
        {
            towerCtrl.Rotation.TowerHead.forward = towerCtrl.Targeting.DirectionToTarget(gunPoint);

            if (hitInfo.transform.TryGetComponent(out Enemy enemy))
            {
                IDamageable damageable = enemy.Core.DamageReceiver;
                damageable.TakeDamage(damage);
            }

            towerCtrl.Visual.CreateOnHitFX(hitInfo.point);
            towerCtrl.Visual.PlayAttackVFX(gunPoint.position, hitInfo.point);
            towerCtrl.Visual.ReloadVFX(attackCooldown);
            AudioManager.Instance.PlaySFX(attackSFX, true);
        }
    }
}