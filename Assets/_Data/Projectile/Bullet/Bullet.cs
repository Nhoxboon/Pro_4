using System;
using UnityEngine;

public class Bullet : Projectile
{
    protected Vector3 target;
    protected IDamageable damageable;
    protected float speed;
    protected float threshold = 0.1f;

    protected string onHitFX = "BulletHitFX";
    [SerializeField] protected TrailRenderer tr;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTrailRenderer();
    }

    protected void LoadTrailRenderer()
    {
        if (tr != null) return;
        tr = GetComponentInChildren<TrailRenderer>();
        DebugTool.Log(transform.name + " :LoadTrailRenderer", gameObject);
    }

    public void SetupBullet(Vector3 targetPosition, IDamageable newDamageable, float newDamage, float newSpeed)
    {
        tr.Clear();
        target = targetPosition;
        damageable = newDamageable;
        damage = newDamage;
        speed = newSpeed;
    }

    protected void Update() => MoveBullet();

    protected void MoveBullet()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if ((transform.position - target).sqrMagnitude <= threshold * threshold)
        {
            damageable.TakeDamage(damage);
            ProjectileSpawner.Instance.Despawn(gameObject);
            SpawnOnHitFX();
        }
    }

    protected override void SpawnOnHitFX()
    {
        var hitFX = FXSpawner.Instance.Spawn(onHitFX, transform.position, Quaternion.identity);
        hitFX.gameObject.SetActive(true);
    }
}