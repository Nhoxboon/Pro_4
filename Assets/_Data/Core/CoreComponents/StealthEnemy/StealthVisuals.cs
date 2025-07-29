using UnityEngine;

public class StealthVisuals : Visuals
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSmokeFX();
    }

    protected void LoadSmokeFX()
    {
        if (smokeFX != null) return;
        smokeFX = visuals.Find("FX/SmokeScreen").GetComponent<ParticleSystem>();
        DebugTool.Log(transform.name + " :LoadSmokeFX", gameObject);
    }
}