using UnityEngine;

public abstract class TowerAttack : TowerComponent
{
    [SerializeField] protected float attackCooldown = 2f;
    public float AttackCooldown => attackCooldown;
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

    protected virtual bool CanAttack() =>
        Time.time > lastAttackTime + attackCooldown && towerCtrl.Targeting.CurrentTarget is not null;

    protected virtual void Attack() => lastAttackTime = Time.time;

    public void DoAttack()
    {
        if(CanAttack()) Attack();
    }
    
    public virtual void ResetAttack()
    {
        lastAttackTime = 0f;
        if (gunPoint != null)
            gunPoint.localRotation = Quaternion.identity;
    }
}