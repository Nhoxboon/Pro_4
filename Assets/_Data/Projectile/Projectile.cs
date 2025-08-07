using System;
using UnityEngine;

public class Projectile : NhoxBehaviour
{
    [SerializeField] protected Rigidbody rb;
    protected float damage;
    [SerializeField] protected float damageRadius = 0.8f;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
        LoadLayerMask();
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        DebugTool.Log(transform.name + " :LoadRigidbody", gameObject);
    }

    protected void LoadLayerMask()
    {
        if (whatIsEnemy != 0) return;
        whatIsEnemy = LayerMask.GetMask("Enemy");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    public void SetupProjectile(Vector3 newVelocity, float newDamage)
    {
        rb.linearVelocity = newVelocity;
        damage = newDamage;
    }

    protected void DamageEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius, whatIsEnemy);
        foreach (var col in colliders)
        {
            if (!col.TryGetComponent(out Enemy enemy)) continue;
            IDamageable damageable = enemy.Core.DamageReceiver;
            damageable.TakeDamage(damage);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        DamageEnemies();
        ProjectileSpawner.Instance.Despawn(gameObject);
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, damageRadius);
}