using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [SerializeField] protected Core core;

    

    protected void OnEnable()
    {
        ResetEnemy();
    }
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
    }

    protected void LoadCore()
    {
        if (core != null) return;
        core = transform.GetComponentInChildren<Core>();
        Debug.Log(transform.name + " LoadCore", gameObject);
    }

    protected void Update()
    {
        core.LogicUpdate();
    }
    
    public void ResetEnemy()
    {
        core.Movement.ResetMovement();
        core.Stats.Health.Init();
    }
}