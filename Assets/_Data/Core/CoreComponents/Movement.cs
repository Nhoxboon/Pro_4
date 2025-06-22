using System;
using UnityEngine;
using UnityEngine.AI;

public class Movement : CoreComponent
{
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected int currentWpIndex;
    [SerializeField] protected NavMeshAgent agent;
    
    protected float totalDistance;
    
    protected void OnEnable()
    {
        CollectTotalDistance();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNavMeshAgent();
    }

    protected void LoadNavMeshAgent()
    {
        if (agent != null) return;
        agent = core.Root.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
        Debug.Log(transform.name +" LoadNavMeshAgent", gameObject);
    }

    public override void LogicUpdate()
    {
        DistanceToFinishLine();
        MoveToWaypoint();
    }

    protected void MoveToWaypoint()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        FaceTarget(agent.steeringTarget);
        if (agent.remainingDistance < 0.2f)
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }

    protected void FaceTarget(Vector3 newTarget)
    {
        if (core.Root == null) return;

        Vector3 dirToTarget = newTarget - core.Root.transform.position;
        dirToTarget.y = 0; // Keep the direction horizontal

        Quaternion newRotation = Quaternion.LookRotation(dirToTarget);

        core.Root.transform.rotation =
            Quaternion.Slerp(core.Root.transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    protected Vector3 GetNextWaypoint()
    {
        if (currentWpIndex >= WaypointManager.Instance.Waypoints.Length) return core.Root.transform.position;

        Vector3 targetPoint = WaypointManager.Instance.Waypoints[currentWpIndex].position;

        if (currentWpIndex > 0)
        {
            float distance = Vector3.Distance(WaypointManager.Instance.Waypoints[currentWpIndex].position,
                WaypointManager.Instance.Waypoints[currentWpIndex - 1].position);
            totalDistance -= distance;
        }
        
        currentWpIndex++;

        return targetPoint;
    }
    
    protected void CollectTotalDistance()
    {
        for (int i = 0; i < WaypointManager.Instance.Waypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(
                WaypointManager.Instance.Waypoints[i].position,
                WaypointManager.Instance.Waypoints[i + 1].position);
            totalDistance += distance;
        }
    }
    
    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;

    public void ResetMovement()
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