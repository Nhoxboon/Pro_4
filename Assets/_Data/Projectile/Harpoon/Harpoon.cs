using System;
using UnityEngine;

public class Harpoon : Projectile
{
    protected bool isAttached;
    protected float speed;
    protected Enemy target;
    protected Transform tower;
    
    [SerializeField] protected Transform connectionPoint;

    protected void Update()
    {
        if (target is null || isAttached) return;
        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, target.transform.position) < 0.35f)
            AttachToTarget();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadConnectionPoint();
    }
    
    protected void LoadConnectionPoint()
    {
        if (connectionPoint != null) return;
        connectionPoint = transform.Find("ConnectionPoint");
        DebugTool.Log(transform.name + " :LoadConnectionPoint", gameObject);   
    }

    public void SetupHarpoon(Enemy newEnemy, float newSpeed, Transform newTower)
    {
        target = newEnemy;
        speed = newSpeed;
        tower = newTower;
    }

    protected void MoveTowardsTarget()
    {
        if (target is null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        Vector3 direction = target.transform.position - transform.position;
        if (direction.sqrMagnitude > 0.001f)
            transform.forward = direction;
    }

    protected void AttachToTarget()
    {
        if (target is null) return;
        isAttached = true;
        transform.parent = target.transform;
        // if (target.Core.Death is FlyDeath flyDeath)
        //     flyDeath.AddAttachedHarpoon(this);

        if (tower.TryGetComponent(out TowerCtrl towerCtrl) && towerCtrl.Attack is HarpoonAttack harpoonAttack)
            harpoonAttack.ActivateAttack();
    }

    public void ResetHarpoon(Vector3 newPos, Quaternion newRot, Transform towerHead )
    {
        isAttached = false;
        target = null;
        tower = null;
        transform.SetParent(towerHead);
        transform.position = newPos;
        transform.rotation = newRot;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    public Transform GetConnectionPoint() => connectionPoint;
}