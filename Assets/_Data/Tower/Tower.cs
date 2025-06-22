using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Tower : NhoxBehaviour
{
    public Enemy currentTarget;
    [SerializeField] protected float attacleCooldown = 2f;
    protected float lastAttackTime;

    [Header("Tower Setup")] [SerializeField]
    protected Transform towerHead;
    [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;
    [Header("Rotation Settings")]
    [SerializeField] protected bool canRotate;
    [SerializeField] protected float rotationSpeed = 10f;
    [Header("Attack Settings")]
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected Vector3 AttackCenter => transform.position - Vector3.up * 1.5f;

    protected virtual void Update()
    {
        if (currentTarget is null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindRandomTargetWithinRange();
            return;
        }

        if (CanAttack()) Attack();

        if (Vector3.Distance(AttackCenter, currentTarget.GetCenterPoint()) > attackRange) currentTarget = null;

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
        if (!canRotate || currentTarget is null || !currentTarget.gameObject.activeInHierarchy) return;

        Vector3 dirToTarget = DirectionToTarget(towerHead);
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Vector3 rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime)
            .eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected Enemy FindRandomTargetWithinRange()
    {
        List<Enemy> priorityTargets = new List<Enemy>();
        List<Enemy> possibleTargets = new List<Enemy>();
        
        foreach (Collider enemy in Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy))
        {
            if (!enemy.TryGetComponent<Enemy>(out var newEnemy)) continue;
            
            (newEnemy.GetEnemyType() == enemyPriorityType ? priorityTargets : possibleTargets).Add(newEnemy);
        }

        return GetMostAdvancedEnemy(priorityTargets.Count > 0 ? priorityTargets : possibleTargets);
    }

    protected Enemy GetMostAdvancedEnemy(List<Enemy> targets)
    {
        Enemy mostAdvancedEnemy = null;
        float minRemainingDistance = float.MaxValue;
        
        foreach (Enemy enemy in targets)
        {
            if (!enemy.gameObject.activeInHierarchy) continue;

            float remainingDistance = enemy.Core.Movement.DistanceToFinishLine();
            if (remainingDistance < minRemainingDistance)
            {
                minRemainingDistance = remainingDistance;
                mostAdvancedEnemy = enemy;
            }
        }
        return mostAdvancedEnemy;
    }

    protected Vector3 DirectionToTarget(Transform startPoint)
    {
        if (currentTarget is null || !currentTarget.gameObject.activeInHierarchy) return Vector3.zero;
        return (currentTarget.GetCenterPoint() - startPoint.position).normalized;
    }

    public void EnableRotation(bool enable) => canRotate = enable;
    
    protected virtual void OnDrawGizmos() => Gizmos.DrawWireSphere(AttackCenter, attackRange);
    
}