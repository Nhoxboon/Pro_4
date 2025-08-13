using System;
using UnityEngine;

public abstract class Projectile : NhoxBehaviour
{
    protected float damage;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected void OnEnable() => ResetProjectile();

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

    protected virtual void ResetProjectile()
    {
        //For override
    }

    protected virtual void SpawnOnHitFX()
    {
        // For override
    }
}