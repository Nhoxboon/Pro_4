using UnityEngine;

public abstract class TowerRotation : TowerComponent
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

    protected abstract void LoadTowerHead();

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
    
    public virtual void ResetRotation()
    {
        if (towerHead != null)
            towerHead.localRotation = Quaternion.identity;
    }
}