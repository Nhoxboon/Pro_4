using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : TowerComponent
{
    [SerializeField] protected float attackRange = 3.5f;
    [SerializeField] protected EnemyType[] enemyPriorityType;
    [SerializeField] protected bool dynamicTargetChange = true;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected float targetCheckInterval = 0.1f;
    protected float lastTimeCheckedTarget;
    public Enemy CurrentTarget { get; protected set; }
    public float AttackRange => attackRange;
    public Vector3 AttackCenter => towerCtrl.transform.position - Vector3.up * 1.5f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyLayerMask();
    }

    protected void LoadEnemyLayerMask()
    {
        if (whatIsEnemy != 0) return;
        whatIsEnemy = LayerMask.GetMask("Enemy");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    public void UpdateTarget()
    {
        if (CurrentTarget is null || !CurrentTarget.gameObject.activeInHierarchy)
        {
            CurrentTarget = FindRandomTargetWithinRange();
            return;
        }

        if (!dynamicTargetChange) return;

        if (Time.time > lastTimeCheckedTarget + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            CurrentTarget = FindRandomTargetWithinRange();
        }
    }

    public virtual void LoseTarget()
    {
        if (CurrentTarget is null || !CurrentTarget.gameObject.activeInHierarchy) return;
        if (Vector3.Distance(AttackCenter, CurrentTarget.GetCenterPoint()) > attackRange)
            CurrentTarget = null;
    }

    protected virtual Enemy FindRandomTargetWithinRange()
    {
        List<Enemy> priorityTargets = new List<Enemy>();
        List<Enemy> possibleTargets = new List<Enemy>();

        Collider[] enemies = Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy);
        foreach (var enemy in enemies)
        {
            if (!enemy.TryGetComponent<Enemy>(out var newEnemy)) continue;

            bool isPriority = Array.Exists(enemyPriorityType, t => t == newEnemy.GetEnemyType());
            (isPriority ? priorityTargets : possibleTargets).Add(newEnemy);
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

    public Vector3 DirectionToTarget(Transform startPoint)
    {
        if (CurrentTarget is null || !CurrentTarget.gameObject.activeInHierarchy) return Vector3.zero;
        return (CurrentTarget.GetCenterPoint() - startPoint.position).normalized;
    }
    
    public bool AtLeastOneTargetInRange()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy);
        return enemyColliders.Length > 0;
    }
    
    public virtual void ResetTargeting() => CurrentTarget = null;

    protected virtual void OnDrawGizmos() => Gizmos.DrawWireSphere(AttackCenter, attackRange);
}