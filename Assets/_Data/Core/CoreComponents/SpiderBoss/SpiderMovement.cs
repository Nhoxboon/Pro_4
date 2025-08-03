using UnityEngine;

public class SpiderMovement : Movement
{
    protected override void Awake()
    {
        base.Awake();
        SpeedUpSpiderLegs();
    }

    protected void OnEnable() => SpeedUpSpiderLegs();

    protected override void ChangeWayPoint()
    {
        SpeedUpSpiderLegs();
        base.ChangeWayPoint();
    }

    protected override bool ShouldChangeWaypoint()
    {
        if (nextWpIndex >= myWayPoints.Count) return false;
        return agent.remainingDistance < 0.2f;
    }

    protected void SpeedUpSpiderLegs()
    {
        if(core.Visuals is SpiderVisuals spiderVisuals)
            spiderVisuals.BrieflySpeedUpLegs();
    }
}
