using UnityEngine;

public class CanonRotation : TowerRotation
{
    [Header("Canon Tower Setup")] [SerializeField]
    protected Transform towerBody;

    [SerializeField] protected float timeToTarget = 1.5f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerBody();
    }

    protected override void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.parent.parent.Find("Model/CanonTower/TowerBody/TowerHead");
        DebugTool.Log(transform.name + " :LoadTowerHead", gameObject);
    }

    protected void LoadTowerBody()
    {
        if (towerBody != null) return;
        towerBody = transform.parent.parent.Find("Model/CanonTower/TowerBody");
        DebugTool.Log(transform.name + " :LoadTowerBody", gameObject);
    }

    public override void HandleRotation()
    {
        if (towerCtrl.Targeting.CurrentTarget is null) return;
        RotateBodyTowardsTarget();
        FaceLaunchDirection();
    }

    protected void RotateBodyTowardsTarget()
    {
        if (towerBody is null) return;
        Vector3 directionToTarget = towerCtrl.Targeting.DirectionToTarget(towerBody);
        directionToTarget.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        towerBody.rotation = Quaternion.Slerp(towerBody.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    protected void FaceLaunchDirection()
    {
        Vector3 attackDirection = CalculateLaunchVelocity();
        Quaternion lookRotation = Quaternion.LookRotation(attackDirection);

        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation.x, towerHead.eulerAngles.y, 0);
    }

    public Vector3 CalculateLaunchVelocity()
    {
        Vector3 direction = towerCtrl.Targeting.CurrentTarget.GetCenterPoint() - towerCtrl.Attack.GunPoint.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        Vector3 velocityXZ = directionXZ / timeToTarget;

        float yVelocity = (direction.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget;
        Vector3 launchVelocity = velocityXZ + (Vector3.up * yVelocity);

        return launchVelocity;
    }
}