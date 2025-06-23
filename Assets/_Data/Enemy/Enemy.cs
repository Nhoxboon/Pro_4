using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [SerializeField] protected EnemyType enemyType = EnemyType.None;
    [SerializeField] protected Transform centerPoint;
    [SerializeField] protected Core core;
    public Core Core => core;
    
    protected void OnEnable() => ResetEnemy();
    
    protected void Update() => core.LogicUpdate();
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
        LoadCenterPoint();
        LoadEnemyType();
    }

    protected void LoadCore()
    {
        if (core != null) return;
        core = transform.GetComponentInChildren<Core>();
        Debug.Log(transform.name + " LoadCore", gameObject);
    }

    protected void LoadCenterPoint()
    {
        if (centerPoint != null) return;
        centerPoint = transform.Find("CenterPoint");
        Debug.Log(transform.name + " :LoadCenterPoint", gameObject);
    }
    
    protected void LoadEnemyType()
    {
        if (enemyType != EnemyType.None) return;
        if (System.Enum.TryParse(transform.name, out EnemyType parsedType)) enemyType = parsedType;
        Debug.Log(transform.name + " :LoadEnemyType", gameObject);
    }
    
    public EnemyType GetEnemyType() => enemyType;
    
    public Vector3 GetCenterPoint() => centerPoint.position;
    
    public void ResetEnemy()
    {
        core.Movement.ResetMovement();
        core.Stats.Health.Init();
    }
}