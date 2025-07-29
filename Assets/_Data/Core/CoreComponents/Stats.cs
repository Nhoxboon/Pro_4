using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected Stat health;
    public Stat Health => health;

    [SerializeField] protected Stat shieldAmount;
    public Stat ShieldAmount => shieldAmount;

    protected override void Awake()
    {
        base.Awake();

        health.SetMaxValue(10f);
        health.Init();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEntityStatsDataSO();
    }

    //Todo: Need to define HP for each type of enemy after finish project
    protected void LoadEntityStatsDataSO()
    {
        // if (entityStatsDataSO != null) return;
        // entityStatsDataSO = Resources.Load<EntityStatsDataSO>("Enemies/Stats/" + transform.parent.parent.name + "Stats");
        // DebugTool.Log(transform.name + " LoadEntityStatsDataSO", gameObject);
    }
}