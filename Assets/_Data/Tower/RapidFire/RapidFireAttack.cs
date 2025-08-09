using UnityEngine;

public class RapidFireAttack : TowerAttack
{
    [Header("Rapid Fire Details")] protected string projectileName = "Bullet";
    [SerializeField] protected Transform[] gunPointSet;
    protected int gunPointIndex;
    [SerializeField] protected float damage = 5f;
    [SerializeField] protected float projectileSpeed = 15f;

    protected void OnEnable() => gunPointIndex = 0;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGunPoint();
    }

    protected override void LoadGunPoint()
    {
        if (gunPointSet is { Length: > 0 }) return;
        gunPointSet = new Transform[6];
        for (int i = 0; i < gunPointSet.Length; i++)
            gunPointSet[i] = towerCtrl.Rotation.TowerHead.Find($"tower_machineGun_barrel_{i + 1}/GunPoint_{i + 1}");
        DebugTool.Log(transform.name + " :LoadGunPoint", gameObject);
    }

    protected override void Attack()
    {
        gunPoint = gunPointSet[gunPointIndex];
        Vector3 dirToTarget = towerCtrl.Targeting.DirectionToTarget(gunPoint);

        if (Physics.Raycast(gunPoint.position, dirToTarget, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
        {
            if (!hitInfo.transform.TryGetComponent(out Enemy enemy)) return;
            IDamageable damageable = enemy.Core.DamageReceiver;
            damageable.TakeDamage(damage);

            var projectile = ProjectileSpawner.Instance.Spawn(projectileName, gunPoint.position, gunPoint.rotation);
            projectile.gameObject.SetActive(true);

            projectile.TryGetComponent(out Bullet bullet);
            bullet.SetupBullet(hitInfo.point, damageable, damage, projectileSpeed);

            PlayRecoilVFX(gunPoint);

            base.Attack();
            gunPointIndex = (gunPointIndex + 1) % gunPointSet.Length;
        }
    }

    protected void PlayRecoilVFX(Transform point)
    {
        if (towerCtrl.Visual is RapidFireVisual rapidFireVisual)
            rapidFireVisual.RecoilVFX(point);
    }
}