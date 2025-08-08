using UnityEngine;

public class Fireball : Projectile
{
    protected string explosionFX = "ExplosionFX";
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected float damageRadius = 0.8f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        DebugTool.Log(transform.name + " :LoadRigidbody", gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        DamageEnemies();
        ProjectileSpawner.Instance.Despawn(gameObject);
        SpawnOnHitFX();
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

    protected override void SpawnOnHitFX()
    {
        var explosion = FXSpawner.Instance.Spawn(explosionFX, transform.position, Quaternion.identity);
        explosion.gameObject.SetActive(true);
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, damageRadius);
}