using System;
using UnityEngine;

public class Bullet : Projectile
{
    protected Vector3 target;
    protected IDamageable damageable;
    protected float speed;

    protected string onHitFX = "BulletHitFX";

    public void SetupBullet(Vector3 targetPosition, IDamageable newDamageable, float newDamage, float newSpeed)
    {
        target = targetPosition;
        damageable = newDamageable;
        damage = newDamage;
        speed = newSpeed;
    }

    protected void Update() => MoveBullet();

    protected void MoveBullet()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) <= 0.01f)
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