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

    [SerializeField] protected Death death;
    public Death Death => death;

    #endregion

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRoot();
        LoadEnemy();
        LoadMovement();
        LoadStats();
        LoadDeath();
    }

    protected void LoadRoot()
    {
        if (root != null) return;
        root = transform.parent.gameObject;
        Debug.Log(transform.name + " LoadRoot", gameObject);
    }
    
    protected void LoadEnemy()
    {
        if (enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        Debug.Log(transform.name + " LoadEnemy", gameObject);
    }

    protected void LoadMovement()
    {
        if (movement != null) return;
        movement = GetComponentInChildren<Movement>();
        Debug.Log(transform.name + " LoadMovement", gameObject);
    }

    protected void LoadStats()
    {
        if (stats != null) return;
        stats = GetComponentInChildren<Stats>();
        Debug.Log(transform.name + " LoadStats", gameObject);
    }

    protected void LoadDeath()
    {
        if (death != null) return;
        death = GetComponentInChildren<Death>();
        Debug.Log(transform.name + " LoadDeath", gameObject);
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