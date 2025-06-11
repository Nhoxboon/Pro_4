
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : NhoxBehaviour
{
    public Transform currentTarget;
    
    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected LayerMask whatIsEnemy;
    
    protected Vector3 AttackCenter => transform.position - Vector3.up * 1.5f;

    protected void Update()
    {
        if (currentTarget == null)
        {
            currentTarget = FindRandomTargetWithinRange();
            return;
        }

        if (Vector3.Distance(AttackCenter, currentTarget.position) > attackRange) currentTarget = null;
        
        RotateTowardsTarget();
    }
    
    protected void RotateTowardsTarget()
    {
        if (currentTarget == null) return;
        Vector3 dirToTarget = currentTarget.position - towerHead.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget);
        Vector3 rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

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
    
    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCenter, attackRange);
    }
}
    