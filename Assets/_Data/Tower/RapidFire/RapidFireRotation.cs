using UnityEngine;

public class RapidFireRotation : TowerRotation
{
    [Header("Rapid Fire Details")] [SerializeField]
    protected Vector3 rotationOffset = new Vector3(0, 0.65f, 0);

    protected override void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.parent.parent.Find("Model/RapidFireTower/TowerHead");
        DebugTool.Log(transform.name + " :LoadTowerHead", gameObject);
    }

    protected override void RotateTowardsTarget()
    {
        if (!canRotate || towerHead is null || towerCtrl.Targeting.CurrentTarget is null ||
            !towerCtrl.Targeting.CurrentTarget.gameObject.activeInHierarchy) return;

        Vector3 dirToTarget =
            (towerCtrl.Targeting.CurrentTarget.GetCenterPoint() - rotationOffset) - towerHead.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);

        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;
        towerHead.rotation = Quaternion.Euler(rotation);
    }
}