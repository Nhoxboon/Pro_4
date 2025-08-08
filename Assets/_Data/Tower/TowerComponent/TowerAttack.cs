using UnityEngine;

public abstract class TowerAttack : TowerComponent
{
    [SerializeField] protected float attackCooldown = 2f;
    [SerializeField] protected Transform gunPoint;
    public Transform GunPoint => gunPoint;
    [SerializeField] protected LayerMask whatIsTargetable;
    [SerializeField] protected AudioSource attackSFX;

    protected float lastAttackTime;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTargetLayerMask();
        LoadAttackSFX();
    }

    protected virtual void LoadGunPoint()
    {
        if (gunPoint != null) return;
        gunPoint = towerCtrl.Rotation.TowerHead.Find("GunPoint");
        DebugTool.Log(transform.name + " :LoadGunPoint", gameObject);
    }

    protected void LoadTargetLayerMask()
    {
        if (whatIsTargetable != 0) return;
        whatIsTargetable = LayerMask.GetMask("Default", "Enemy");
        DebugTool.Log(transform.name + " :LoadTargetLayerMask", gameObject);
    }

    protected void LoadAttackSFX()
    {
        if (attackSFX != null) return;
        attackSFX = transform.parent.parent.GetComponentInChildren<AudioSource>();
        DebugTool.Log(transform.name + " :LoadAttackSFX", gameObject);
    }

    public bool CanAttack() =>
        Time.time > lastAttackTime + attackCooldown && towerCtrl.Targeting.CurrentTarget is not null;

    public virtual void Attack() => lastAttackTime = Time.time;
}