using System;
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
    [SerializeField] protected TowerVisuals visuals;
    public TowerVisuals Visuals => visuals;

    [SerializeField] protected List<TowerComponent> components = new();
    public List<TowerComponent> Components => components;

    protected void OnEnable() => ResetTower();

    protected virtual void Update()
    {
        targeting.LoseTarget();
        targeting.UpdateTarget();
        if (!status.IsActive) return;

        attack.DoAttack();

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

    protected virtual void LoadTowerTargeting()
    {
        if (targeting != null) return;
        targeting = GetComponentInChildren<TowerTargeting>();
        DebugTool.Log(transform.name + " :LoadTowerTargeting", gameObject);
    }

    protected virtual void LoadTowerRotation()
    {
        if (rotation != null) return;
        rotation = GetComponentInChildren<TowerRotation>();
        DebugTool.Log(transform.name + " :LoadTowerRotation", gameObject);
    }

    protected virtual void LoadTowerAttack()
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

    protected virtual void LoadTowerVisual()
    {
        if (visuals != null) return;
        visuals = GetComponentInChildren<TowerVisuals>();
        DebugTool.Log(transform.name + " :LoadTowerVisual", gameObject);
    }

    public void AddComponent(TowerComponent component)
    {
        if (!components.Contains(component)) components.Add(component);
    }

    protected virtual void ResetTower()
    {
        if (attack != null)
            attack.ResetAttack();

        if (targeting != null)
            targeting.ResetTargeting();

        if (rotation != null)
            rotation.ResetRotation();

        if (visuals != null)
            visuals.ResetVisual();

        if (status != null)
            status.ResetStatus();
    }
}