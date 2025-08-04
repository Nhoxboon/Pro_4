using UnityEngine;

public class FXDespawnByParticle : Despawn
{
    [SerializeField] private ParticleSystem mainParticle;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadParticle();
    }

    protected void LoadParticle()
    {
        if (mainParticle != null) return;
        mainParticle = transform.parent.GetComponentInChildren<ParticleSystem>();
        DebugTool.Log(transform.name + " :LoadParticle", gameObject);
    }

    protected override bool CanDespawn()
    {
        return !mainParticle.IsAlive(true);
    }

    public override void DespawnObject() => FXSpawner.Instance.Despawn(transform.parent.gameObject);
}