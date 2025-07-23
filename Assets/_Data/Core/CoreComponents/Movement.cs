using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : CoreComponent
{
    [SerializeField] protected float turnSpeed = 10f;

    [SerializeField] protected int currentWpIndex;
    [SerializeField] protected int nextWpIndex;
    [SerializeField] protected List<Transform> myWayPoints;

    [SerializeField] protected EnemyPortal myPortal;
    [SerializeField] protected NavMeshAgent agent;

    protected float totalDistance;

    protected void OnDisable()
    {
        myWayPoints.Clear();
        myPortal = null;
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
        DebugTool.Log(transform.name + " LoadNavMeshAgent", gameObject);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        DistanceToFinishLine();
        MoveToWaypoint();
    }

    public void SetUpEnemy(List<Waypoint> newWaypoints, EnemyPortal newPortal)
    {
        myWayPoints = new List<Transform>();
        foreach (var point in newWaypoints)
        {
            myWayPoints.Add(point.transform);
        }

        CollectTotalDistance();

        if (core.Root.TryGetComponent<Enemy>(out var enemy)) enemy.SetPortal(newPortal);

        ResetMovement();
    }

    protected void MoveToWaypoint()
    {
        if (agent is null || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        FaceTarget(agent.steeringTarget);

        if (ShouldChangeWaypoint()) agent.SetDestination(GetNextWaypoint());
    }

    protected bool ShouldChangeWaypoint()
    {
        if (nextWpIndex >= myWayPoints.Count) return false;
        if (agent.remainingDistance < 0.2f) return true;

        Vector3 currentWaypoint = myWayPoints[currentWpIndex].position;
        Vector3 nextWaypoint = myWayPoints[nextWpIndex].position;

        float distanceToNextWp = Vector3.Distance(core.Root.transform.position, nextWaypoint);
        float distanceBetweenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceBetweenPoints > distanceToNextWp;
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
        if (nextWpIndex >= myWayPoints.Count) return core.Root.transform.position;

        Vector3 targetPoint = myWayPoints[nextWpIndex].position;

        if (nextWpIndex > 0)
        {
            float distance = Vector3.Distance(myWayPoints[nextWpIndex].position,
                myWayPoints[nextWpIndex - 1].position);
            totalDistance -= distance;
        }

        nextWpIndex++;
        currentWpIndex = nextWpIndex - 1;

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
        nextWpIndex = 0;
        totalDistance = 0;

        if (agent is null || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        if (myWayPoints.Count > 0 && myWayPoints[0] is not null)
            agent.SetDestination(myWayPoints[0].position);
    }
}