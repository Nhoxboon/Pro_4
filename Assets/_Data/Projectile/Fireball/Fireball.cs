using UnityEngine;

public class Fireball : Projectile
{
    protected string explosionFX = "ExplosionFX";
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected float damageRadius = 0.85f;
    [SerializeField] protected TrailRenderer tr;
    [SerializeField] protected LayerMask whatIsUnInteractive;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
        LoadTrailRenderer();
        LoadUnInteractiveLayer();
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        DebugTool.Log(transform.name + " :LoadRigidbody", gameObject);
    }

    protected void LoadTrailRenderer()
    {
        if (tr != null) return;
        tr = GetComponentInChildren<TrailRenderer>();
        DebugTool.Log(transform.name + " :LoadTrailRenderer", gameObject);
    }

    protected void LoadUnInteractiveLayer()
    {
        if (whatIsUnInteractive != 0) return;
        whatIsUnInteractive = LayerMask.GetMask("EnemyProjectile", "FlyRoad");
        DebugTool.Log(transform.name + " :LoadUnInteractiveLayer", gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer & whatIsUnInteractive) != 0) return;
        DamageEnemies();
        ProjectileSpawner.Instance.Despawn(gameObject);
        SpawnOnHitFX();
    }

    public void SetupProjectile(Vector3 newVelocity, float newDamage)
    {
        tr.Clear();
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
        var explosion = FXSpawner.Instance.Spawn(explosionFX, transform.position + new Vector3(0, 0.5f, 0),
            Quaternion.identity);
        explosion.gameObject.SetActive(true);
    }

    // private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, damageRadius);
}