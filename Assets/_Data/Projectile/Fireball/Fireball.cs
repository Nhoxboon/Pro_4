using UnityEngine;

public class Fireball : Projectile
{
    protected string explosionFX = "ExplosionFX";

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        SpawnExplosionFX();
    }

    protected void SpawnExplosionFX()
    {
        var explosion = FXSpawner.Instance.Spawn(explosionFX, transform.position, Quaternion.identity);
        explosion.gameObject.SetActive(true);
    }
}
