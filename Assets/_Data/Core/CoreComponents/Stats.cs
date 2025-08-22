using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected Stat health;
    public Stat Health => health;

    [SerializeField] protected Stat shieldAmount;
    public Stat ShieldAmount => shieldAmount;

    [SerializeField] protected EnemyDataSO enemyStatsDataSO;
    public EnemyDataSO EnemyStatsDataSO => enemyStatsDataSO;

    protected override void Awake()
    {
        base.Awake();

        health.SetMaxValue(enemyStatsDataSO.health);
        health.Init();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEntityStatsDataSO();
    }

    protected void LoadEntityStatsDataSO()
    {
        if (enemyStatsDataSO != null) return;
        enemyStatsDataSO = Resources.Load<EnemyDataSO>("Enemy/" + core.Root.transform.name);
        DebugTool.Log(transform.name + " LoadEntityStatsDataSO", gameObject);
    }
}