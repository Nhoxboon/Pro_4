using UnityEngine;

public class TowerRotation : TowerComponent
{
    [SerializeField] protected Transform towerHead;
    public Transform TowerHead => towerHead;
    [SerializeField] protected float rotationSpeed = 10f;

    protected bool canRotate = true;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerHead();
    }

    protected void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.parent.Find("Model/CrossbowTower/TowerHead");
        DebugTool.Log(transform.name + " :LoadTowerHead", gameObject);
    }

    public virtual void HandleRotation() => RotateTowardsTarget();

    protected virtual void RotateTowardsTarget()
    {
        if (!canRotate || towerHead is null || towerCtrl.Targeting.CurrentTarget is null ||
            !towerCtrl.Targeting.CurrentTarget.gameObject.activeInHierarchy) return;

        Vector3 dirToTarget = towerCtrl.Targeting.DirectionToTarget(towerHead);
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Vector3 rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation);
    }

    public void EnableRotation(bool enable) => canRotate = enable;
}