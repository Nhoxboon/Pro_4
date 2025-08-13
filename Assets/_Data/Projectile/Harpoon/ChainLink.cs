using UnityEngine;

public class ChainLink : Projectile
{
    [SerializeField] protected LineRenderer lr;
    [SerializeField] protected MeshRenderer mr;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLineRenderer();
        LoadMeshRenderer();
    }

    protected void LoadLineRenderer()
    {
        if (lr != null) return;
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        DebugTool.Log(transform.name + " :LoadLineRenderer", gameObject);
    }

    protected void LoadMeshRenderer()
    {
        if (mr != null) return;
        mr = GetComponentInChildren<MeshRenderer>();
        DebugTool.Log(transform.name + " :LoadMeshRenderer", gameObject);
    }

    public void PositionLink( Vector3 newPosition) => transform.position = newPosition;
    
    public void UpdateLineRenderer(ChainLink startPoint, ChainLink endPoint)
    {
        lr.enabled = startPoint.CurrentlyActive() && endPoint.CurrentlyActive();

        if (!lr.enabled) return;
        lr.SetPosition(0, startPoint.transform.position);
        lr.SetPosition(1, endPoint.transform.position);
    }

    protected bool CurrentlyActive() => gameObject.activeSelf;

    protected override void ResetProjectile() => lr.enabled = false;

    public void DespawnLink()
    {
        ProjectileSpawner.Instance.BackToHolder(gameObject);
        ProjectileSpawner.Instance.Despawn(gameObject);
    }
}