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
        Debug.Log(transform.name + " :LoadGunPoint", gameObject);
    }

    protected void LoadCrossbowVisual()
    {
        if (visual != null) return;
        visual = GetComponentInChildren<CrossbowVisual>();
        Debug.Log(transform.name + " :LoadCrossbowVisual", gameObject);
    }

    protected override void Attack()
    {
        if (Physics.Raycast(gunPoint.position, DirectionToTarget(gunPoint), out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = DirectionToTarget(gunPoint);
            // Debug.DrawLine(gunPoint.position, hitInfo.point);

            if (hitInfo.collider.TryGetComponentInChildren<IDamageable>(out IDamageable damageable))
                damageable.TakeDamage(damage);

            visual.PlayAttackVFX(gunPoint.position, hitInfo.point, currentTarget);
            visual.ReloadVFX(attackCooldown);
        }
    }
}