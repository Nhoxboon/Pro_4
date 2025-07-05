using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : CoreComponent
{
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected int currentWpIndex;
    [SerializeField] protected List<Transform> myWayPoints;
    [SerializeField] protected NavMeshAgent agent;

    protected float totalDistance;

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
        Debug.Log(transform.name + " LoadNavMeshAgent", gameObject);
    }

    public override void LogicUpdate()
    {
        DistanceToFinishLine();
        MoveToWaypoint();
    }

    public void SetUpPath(List<Waypoint> newWaypoints)
    {
        myWayPoints = new List<Transform>();
        foreach (var point in newWaypoints)
        {
            myWayPoints.Add(point.transform);
        }

        CollectTotalDistance();

        ResetMovement();
    }

    protected void MoveToWaypoint()
    {
        if (agent is null || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        FaceTarget(agent.steeringTarget);
        if (agent.remainingDistance < 0.2f)
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }

    protected void FaceTarget(Vector3 newTarget)
    {
        if (core.Root is null) return;

        Vector3
            dirToTarget =
                newTarget - core.Root.transform
                    .position; //or use agent.velocity.normalized but it may not be accurate in some cases
        dirToTarget.y = 0; // Keep the direction horizontal

        Quaternion newRotation = Quaternion.LookRotation(dirToTarget);

        core.Root.transform.rotation =
            Quaternion.Slerp(core.Root.transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    protected Vector3 GetNextWaypoint()
    {
        if (currentWpIndex >= myWayPoints.Count) return core.Root.transform.position;

        Vector3 targetPoint = myWayPoints[currentWpIndex].position;

        if (currentWpIndex > 0)
        {
            float distance = Vector3.Distance(myWayPoints[currentWpIndex].position,
                myWayPoints[currentWpIndex - 1].position);
            totalDistance -= distance;
        }

        currentWpIndex++;

        return targetPoint;
    }

    protected void CollectTotalDistance()
    {
        for (int i = 0; i < myWayPoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(
                myWayPoints[i].position,
                myWayPoints[i + 1].position);
            totalDistance += distance;
        }
    }

    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;

    public void ResetMovement()
    {
        currentWpIndex = 0;

        if (agent is null || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        if (myWayPoints.Count > 0)
        {
            agent.SetDestination(myWayPoints[0].position);
        }
    }
}