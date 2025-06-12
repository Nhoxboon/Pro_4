using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Tower : NhoxBehaviour
{
    public Transform currentTarget;
    [SerializeField] protected float attacleCooldown = 1f;
    protected float lastAttackTime;

    [Header("Tower Setup")] [SerializeField]
    protected Transform towerHead;
    [SerializeField] protected bool canRotate;

    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected Vector3 AttackCenter => transform.position - Vector3.up * 1.5f;

    protected virtual void Update()
    {
        if (currentTarget == null)
        {
            currentTarget = FindRandomTargetWithinRange();
            return;
        }

        if (CanAttack()) Attack();

        if (Vector3.Distance(AttackCenter, currentTarget.position) > attackRange) currentTarget = null;

        RotateTowardsTarget();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerHead();
        LoadLayerMask();
    }

    protected void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.Find("Model/CrossbowTower/TowerHead");
        Debug.Log(transform.name + " :LoadTowerHead", gameObject);
    }

    protected void LoadLayerMask()
    {
        if (whatIsEnemy != 0) return;
        whatIsEnemy = LayerMask.GetMask("Enemy");
        Debug.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    protected bool CanAttack()
    {
        if (Time.time > lastAttackTime + attacleCooldown)
        {
            lastAttackTime = Time.time;
            return true;
        }

        return false;
    }

    protected abstract void Attack();

    protected virtual void RotateTowardsTarget()
    {
        if (!canRotate) return;
        
        if (currentTarget == null) return;
        
        Vector3 dirToTarget = currentTarget.position - towerHead.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Vector3 rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected Transform FindRandomTargetWithinRange()
    {
        List<Transform> possibleTargets = new List<Transform>();
        Collider[] enemiesAround = Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy);
        foreach (Collider enemy in enemiesAround)
        {
            possibleTargets.Add(enemy.transform);
        }

        if (possibleTargets.Count == 0) return null;

        int ramdomIndex = Random.Range(0, possibleTargets.Count);
        return possibleTargets[ramdomIndex];
    }

    protected Vector3 DirectionToTarget(Transform startPoint)
    {
        if (currentTarget == null) return Vector3.zero;
        return (currentTarget.position - startPoint.position).normalized;
    }
    
    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCenter, attackRange);
    }
}