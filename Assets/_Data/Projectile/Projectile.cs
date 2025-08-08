using UnityEngine;

public abstract class Projectile : NhoxBehaviour
{
    protected float damage;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLayerMask();
    }

    protected void LoadLayerMask()
    {
        if (whatIsEnemy != 0) return;
        whatIsEnemy = LayerMask.GetMask("Enemy");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    protected abstract void SpawnOnHitFX();
}