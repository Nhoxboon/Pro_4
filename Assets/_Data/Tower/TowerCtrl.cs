using System.Collections.Generic;
using UnityEngine;

public class TowerCtrl : NhoxBehaviour
{
    [SerializeField] protected TowerTargeting targeting;
    public TowerTargeting Targeting => targeting;
    [SerializeField] protected TowerRotation rotation;
    public TowerRotation Rotation => rotation;
    [SerializeField] protected TowerAttack attack;
    public TowerAttack Attack => attack;
    [SerializeField] protected TowerStatus status;
    public TowerStatus Status => status;
    [SerializeField] protected TowerVisual visual;
    public TowerVisual Visual => visual;

    [SerializeField] protected List<TowerComponent> components = new();
    public List<TowerComponent> Components => components;

    protected virtual void Update()
    {
        targeting.LoseTarget();
        targeting.UpdateTarget();
        if (!status.IsActive) return;

        if (attack.CanAttack())
            attack.Attack();

        rotation.HandleRotation();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerTargeting();
        LoadTowerRotation();
        LoadTowerAttack();
        LoadTowerStatus();
        LoadTowerVisual();
    }

    protected void LoadTowerTargeting()
    {
        if (targeting != null) return;
        targeting = GetComponentInChildren<TowerTargeting>();
        DebugTool.Log(transform.name + " :LoadTowerTargeting", gameObject);
    }

    protected void LoadTowerRotation()
    {
        if (rotation != null) return;
        rotation = GetComponentInChildren<TowerRotation>();
        DebugTool.Log(transform.name + " :LoadTowerRotation", gameObject);
    }

    protected void LoadTowerAttack()
    {
        if (attack != null) return;
        attack = GetComponentInChildren<TowerAttack>();
        DebugTool.Log(transform.name + " :LoadTowerAttack", gameObject);
    }

    protected void LoadTowerStatus()
    {
        if (status != null) return;
        status = GetComponentInChildren<TowerStatus>();
        DebugTool.Log(transform.name + " :LoadTowerStatus", gameObject);
    }

    protected void LoadTowerVisual()
    {
        if (visual != null) return;
        visual = GetComponentInChildren<TowerVisual>();
        DebugTool.Log(transform.name + " :LoadTowerVisual", gameObject);
    }

    public void AddComponent(TowerComponent component)
    {
        if (!components.Contains(component)) components.Add(component);
    }
}
