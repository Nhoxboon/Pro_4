using UnityEngine;
using UnityEngine.AI;

public class Enemy : NhoxBehaviour
{
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected Transform[] waypoints;
    [SerializeField] protected int currentWpIndex;
    [SerializeField] protected NavMeshAgent agent;

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

    protected void Update()
    {
        EnemyMove();
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
        if (currentWpIndex >= waypoints.Length) return transform.position;

        Vector3 targetPoint = waypoints[currentWpIndex].position;
        currentWpIndex++;

        return targetPoint;
    }
}