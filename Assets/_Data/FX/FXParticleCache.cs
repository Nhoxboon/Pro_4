
using UnityEngine;

public class FXParticleCache : NhoxBehaviour
{
    public ParticleSystem cachedParticle;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCachedParticle();
    }

    protected void LoadCachedParticle()
    {
        if(cachedParticle != null) return;
        cachedParticle = GetComponentInChildren<ParticleSystem>(true);
        DebugTool.Log(transform.name + " :LoadCachedParticle", gameObject);
    }
}
