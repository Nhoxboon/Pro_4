using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : NhoxBehaviour
{
    [SerializeField] protected List<CoreComponent> components = new();

    #region Core Components

    [SerializeField] protected GameObject root;
    public GameObject Root => root;

    [SerializeField] protected Enemy enemy;
    public Enemy Enemy => enemy;

    [SerializeField] protected Movement movement;
    public Movement Movement => movement;

    [SerializeField] protected Stats stats;
    public Stats Stats => stats;

    [SerializeField] protected DamageReceiver damageReceiver;
    public DamageReceiver DamageReceiver => damageReceiver;

    [SerializeField] protected Visuals visuals;
    public Visuals Visuals => visuals;

    [SerializeField] protected Death death;
    public Death Death => death;

    [SerializeField] protected ShieldForEnemy shieldObject;
    public ShieldForEnemy ShieldObject => shieldObject;

    [SerializeField] protected SpawnUnit spawnUnit;
    public SpawnUnit SpawnUnit => spawnUnit;

    #endregion

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRoot();
        LoadEnemy();
        LoadMovement();
        LoadStats();
        LoadDamageReceiver();
        LoadVisuals();
        LoadDeath();
    }

    protected void LoadRoot()
    {
        if (root != null) return;
        root = transform.parent.gameObject;
        DebugTool.Log(transform.name + " LoadRoot", gameObject);
    }

    protected void LoadEnemy()
    {
        if (enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        DebugTool.Log(transform.name + " LoadEnemy", gameObject);
    }

    protected void LoadMovement()
    {
        if (movement != null) return;
        movement = GetComponentInChildren<Movement>();
        DebugTool.Log(transform.name + " LoadMovement", gameObject);
    }

    protected void LoadStats()
    {
        if (stats != null) return;
        stats = GetComponentInChildren<Stats>();
        DebugTool.Log(transform.name + " LoadStats", gameObject);
    }

    protected void LoadDamageReceiver()
    {
        if (damageReceiver != null) return;
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        DebugTool.Log(transform.name + " LoadDamageReceiver", gameObject);
    }

    protected void LoadVisuals()
    {
        if (visuals != null) return;
        visuals = GetComponentInChildren<Visuals>();
        DebugTool.Log(transform.name + " LoadVisuals", gameObject);
    }

    protected void LoadDeath()
    {
        if (death != null) return;
        death = GetComponentInChildren<Death>();
        DebugTool.Log(transform.name + " LoadDeath", gameObject);
    }

    public void LogicUpdate()
    {
        foreach (var component in components) component.LogicUpdate();
    }

    public void AddComponent(CoreComponent component)
    {
        if (!components.Contains(component)) components.Add(component);
    }
}