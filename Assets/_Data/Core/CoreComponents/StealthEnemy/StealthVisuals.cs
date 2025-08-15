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
        switch (enable)
        {
            case true when !smokeFX.isPlaying:
                smokeFX.Play();
                break;
            case false when smokeFX.isPlaying:
                smokeFX.Stop();
                break;
        }
    }
    
    public override void ResetVisuals()
    {
        AlignWithSlope();
        EnableSmoke(true);
    }
}