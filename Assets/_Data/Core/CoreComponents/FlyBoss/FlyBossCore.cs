using UnityEngine;

public class FlyBossCore : Core
{
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