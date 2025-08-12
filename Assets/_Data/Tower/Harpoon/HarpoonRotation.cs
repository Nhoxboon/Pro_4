using UnityEngine;

public class HarpoonRotation : TowerRotation
{
    [SerializeField] protected Transform towerBody;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerBody();
    }

    protected override void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.parent.parent.Find("Model/HarpoonTower/TowerBody/TowerHead");
    }

    protected void LoadTowerBody()
    {
        if (towerBody != null) return;
        towerBody = transform.parent.parent.Find("Model/HarpoonTower/TowerBody");
        DebugTool.Log(transform.name + " :LoadTowerBody", gameObject);
    }

    public override void HandleRotation()
    {
        base.HandleRotation();
        RotateBodyTowardsTarget();
    }

    protected void RotateBodyTowardsTarget()
    {
        if (towerBody is null || towerCtrl.Targeting.CurrentTarget is null) return;
        Vector3 directionToTarget = towerCtrl.Targeting.DirectionToTarget(towerBody);
        directionToTarget.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        towerBody.rotation = Quaternion.Slerp(towerBody.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public override void ResetRotation()
    {
        base.ResetRotation();
        if (towerBody != null) towerBody.rotation = Quaternion.identity;
    }
}