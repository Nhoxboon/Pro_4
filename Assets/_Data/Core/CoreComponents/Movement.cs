using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : CoreComponent
{
    [SerializeField] protected float turnSpeed = 10f;

    [SerializeField] protected int currentWpIndex;
    [SerializeField] protected int nextWpIndex;
    [SerializeField] protected Vector3[] myWayPoints;

    [SerializeField] protected NavMeshAgent agent;

    protected float originalSpeed;
    protected float totalDistance;

    protected override void Awake()
    {
        base.Awake();
        originalSpeed = agent.speed;
    }

    protected virtual void OnDisable()
    {
        agent.speed = originalSpeed;
        agent.enabled = false;
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

    public void SetUpEnemy(EnemyPortal newPortal)
    {
        agent.enabled = true;
        core.Enemy.SetPortal(newPortal);
        UpdateWaypoints(core.Enemy.MyPortal.currentWaypoints);

        CollectTotalDistance();
        ResetMovement();
    }

    protected void UpdateWaypoints(Vector3[] newWayPoints)
    {
        myWayPoints = new Vector3[newWayPoints.Length];
        for (int i = 0; i < myWayPoints.Length; i++)
            myWayPoints[i] = newWayPoints[i];
    }

    protected void MoveToWaypoint()
    {
        if (IsAgentInvalid()) return;

        FaceTarget(agent.steeringTarget);

        if (ShouldChangeWaypoint()) ChangeWayPoint();
    }

    protected virtual void ChangeWayPoint() => agent.SetDestination(GetNextWaypoint());

    protected virtual bool ShouldChangeWaypoint()
    {
        if (nextWpIndex >= myWayPoints.Length) return false;
        if (agent.remainingDistance < 0.4f) return true;

        Vector3 currentWaypoint = myWayPoints[currentWpIndex];
        Vector3 nextWaypoint = myWayPoints[nextWpIndex];

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
        if (nextWpIndex >= myWayPoints.Length) return core.Root.transform.position;

        Vector3 targetPoint = myWayPoints[nextWpIndex];

        if (nextWpIndex > 0)
        {
            float distance = Vector3.Distance(myWayPoints[nextWpIndex],
                myWayPoints[nextWpIndex - 1]);
            totalDistance -= distance;
        }

        nextWpIndex++;
        currentWpIndex = nextWpIndex - 1;

        return targetPoint;
    }

    public Vector3 GetFinalWaypoint() =>
        myWayPoints.Length == 0 ? core.Root.transform.position : myWayPoints[^1];

    protected void CollectTotalDistance()
    {
        for (int i = 0; i < myWayPoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(
                myWayPoints[i],
                myWayPoints[i + 1]);
            totalDistance += distance;
        }
    }

    public virtual float DistanceToFinishLine() =>
        IsAgentInvalid() ? totalDistance : totalDistance + agent.remainingDistance;

    public void SlowEnemy(float slowMultiplier, float duration)
    {
        if (IsAgentInvalid()) return;
        StartCoroutine(SlowEnemyCoroutine(slowMultiplier, duration));
    }

    protected IEnumerator SlowEnemyCoroutine(float slowMultiplier, float duration)
    {
        agent.speed = originalSpeed;
        agent.speed *= slowMultiplier;
        yield return new WaitForSeconds(duration);
        agent.speed = originalSpeed;
    }

    public virtual void ResetMovement()
    {
        currentWpIndex = 0;
        nextWpIndex = 0;
        totalDistance = 0;

        if (IsAgentInvalid()) return;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        if (myWayPoints.Length > 0)
            agent.SetDestination(myWayPoints[0]);
    }

    protected bool IsAgentInvalid() => agent is null || !agent.isActiveAndEnabled || !agent.isOnNavMesh;
}