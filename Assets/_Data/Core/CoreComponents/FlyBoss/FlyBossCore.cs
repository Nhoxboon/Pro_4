using UnityEngine;

public class FlyBossCore : Core
{
    [SerializeField] protected SpawnUnit spawnUnit;
    public SpawnUnit SpawnUnit => spawnUnit;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSpawnUnit();
    }

    protected void LoadSpawnUnit()
    {
        if (spawnUnit != null) return;
        spawnUnit = GetComponentInChildren<SpawnUnit>();
        DebugTool.Log(transform.name + " :LoadSpawnUnit", gameObject);
    }
}