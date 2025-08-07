using UnityEngine;

public class CanonAttack : TowerAttack
{
    protected string projectileName = "Fireball";
    protected string onAttackVFX = "SmokeCanonVFX";
    [SerializeField] protected int damage = 2;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
    }

    public override void Attack()
    {
        base.Attack();
        if (towerCtrl.Rotation is not CanonRotation canonRotation) return;
        Vector3 velocity = canonRotation.CalculateLaunchVelocity();
        Transform newProjectile =
            ProjectileSpawner.Instance.Spawn(projectileName, towerCtrl.Attack.GunPoint.position,
                Quaternion.identity);
        newProjectile.gameObject.SetActive(true);

        if (newProjectile.TryGetComponent(out Projectile projectile))
            projectile.SetupProjectile(velocity, damage);
        SpawnAttackVFX();
    }

    protected void SpawnAttackVFX()
    {
        var vfx = FXSpawner.Instance.Spawn(onAttackVFX, towerCtrl.Attack.GunPoint.position, Quaternion.identity);
        vfx.gameObject.SetActive(true);
    }
}