using UnityEngine;

public class SpiderVisuals : Visuals
{
    [Header("Leg Details")] public float legSpeed = 2.5f;

    [SerializeField] protected SpiderLeg[] legs;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLegs();
    }

    protected void LoadLegs()
    {
        if(legs is { Length: > 0 }) return;
        legs = core.Root.GetComponentsInChildren<SpiderLeg>();
        DebugTool.Log(transform.name + " :LoadLegs", gameObject);
        foreach (var leg in legs)
            leg.SetSpiderVisuals(this);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        UpdateSpiderLeg();
    }

    protected void UpdateSpiderLeg()
    {
        for (int i = 0; i < legs.Length; i++)
            legs[i].UpdateLegMovement();
    }
}