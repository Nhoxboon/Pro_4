using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NhoxBehaviour
{
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected int currentWpIndex;
    [SerializeField] protected NavMeshAgent agent;

    protected void OnEnable()
    {
        ResetEnemy();
    }

    protected override void Awake()
    {
        base.Awake();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    protected void Update()
    {
        EnemyMove();
    }
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNavMeshAgent();
    }

    protected void LoadNavMeshAgent()
    {
        if (agent != null) return;
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(transform.name + "LoadNavMeshAgent", gameObject);
    }

    protected void EnemyMove()
    {
        FaceTarget(agent.steeringTarget);
        if (agent.remainingDistance < 0.2f)
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }

    protected void FaceTarget(Vector3 newTarget)
    {
        Vector3 dirToTarget = newTarget - transform.position;
        dirToTarget.y = 0; // Keep the direction horizontal

        Quaternion newRotation = Quaternion.LookRotation(dirToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    protected Vector3 GetNextWaypoint()
    {
        if (currentWpIndex >= WaypointManager.Instance.Waypoints.Length) return transform.position;

        Vector3 targetPoint = WaypointManager.Instance.Waypoints[currentWpIndex].position;
        currentWpIndex++;

        return targetPoint;
    }
    
    public void ResetEnemy()
    {
        currentWpIndex = 0;
    
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        
            if (WaypointManager.Instance.Waypoints.Length > 0)
            {
                agent.SetDestination(WaypointManager.Instance.Waypoints[0].position);
            }
        }
    }
}