using UnityEngine;

public class FlyMovement : Movement
{
    protected override void Start()
    {
        base.Start();
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.SetDestination(GetFinalWaypoint());
    }

    public override float DistanceToFinishLine() => IsAgentInvalid()
        ? totalDistance
        : Vector3.Distance(core.Root.transform.position, GetFinalWaypoint());


    public override void ResetMovement()
    {
        currentWpIndex = 0;
        nextWpIndex = 0;
        totalDistance = 0;

        if (IsAgentInvalid()) return;

        agent.ResetPath();
        agent.velocity = Vector3.zero;

        agent.SetDestination(GetFinalWaypoint());
    }
}