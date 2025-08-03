using UnityEngine;

public class StealthVisuals : Visuals
{
    [SerializeField] protected ParticleSystem smokeFX;

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

    public void EnableSmoke(bool enable)
    {
        if (!enable) return;
        if (!smokeFX.isPlaying)
            smokeFX.Play();
        else
            smokeFX.Stop();
    }
}