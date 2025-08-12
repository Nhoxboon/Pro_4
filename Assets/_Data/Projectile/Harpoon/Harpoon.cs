using System;
using UnityEngine;

public class Harpoon : Projectile
{
    protected bool isAttached;
    protected float speed;
    protected Enemy target;
    protected Transform tower;

    protected void Update()
    {
        if (target is null || isAttached) return;
        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, target.transform.position) < 0.35f)
            AttachToTarget();
    }

    public void SetupHarpoon(Enemy newEnemy, float newSpeed, Transform newTower)
    {
        target = newEnemy;
        speed = newSpeed;
        tower = newTower;
    }

    protected void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.forward = target.transform.position - transform.position;
    }

    protected void AttachToTarget()
    {
        isAttached = true;
        transform.parent = target.transform;
        if (tower.TryGetComponent(out TowerCtrl towerCtrl) && towerCtrl.Attack is HarpoonAttack harpoonAttack)
            harpoonAttack.ActivateAttack();
    }

    protected override void ResetProjectile()
    {
        isAttached = false;
        target = null;
        tower = null;
    }

    protected override void SpawnOnHitFX()
    {
    }
}