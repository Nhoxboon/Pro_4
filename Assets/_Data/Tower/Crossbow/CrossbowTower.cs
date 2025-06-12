using UnityEngine;

public class CrossbowTower : Tower
{
    [Header("Crossbow Tower Setup")] [SerializeField]
    protected Transform gunPoint;

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
        gunPoint = transform.Find("Model/CrossbowTower/GunPoint");
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
            Debug.DrawLine(gunPoint.position, hitInfo.point);

            visual.PlayAttackVFX(gunPoint.position, hitInfo.point);
        }
    }
}