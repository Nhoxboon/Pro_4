using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnCoordinator : WaveSystemManager
{
    private static EnemySpawnCoordinator _instance;
    public static EnemySpawnCoordinator Instance => _instance;

    [SerializeField] protected List<EnemyPortal> enemyPortals = new();
    [SerializeField] protected GridBuilder currentGrid;
    protected bool makingNextWave;

    protected override void SetInstance() => _instance = this;

    protected override void Awake()
    {
        base.Awake();
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
    }

    protected void LoadGridBuilder()
    {
        if (currentGrid != null) return;
        currentGrid = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + ": LoadGridBuilder", gameObject);
    }

    public void HandleWaveCompletion()
    {
        if (!WaveTimingManager.Instance.GameBegun || !AllEnemiesDefeated() || makingNextWave) return;
        makingNextWave = true;

        WaveTimingManager.Instance.AdvanceToNextWave();
        int nextWaveIndex = WaveTimingManager.Instance.CurrentWaveIndex;

        if (WaveTimingManager.Instance.HasNoMoreWaves())
        {
            ManagerCtrl.Instance.GameManager.LevelCompleted();
            return;
        }

        WaveDetails[] levelWave = WaveTimingManager.Instance.LevelWave;

        if (HasNewLayout(nextWaveIndex, levelWave))
            LevelLayoutManager.Instance.UpdateLevelLayout(levelWave[nextWaveIndex], OnLayoutUpdateCompleted);
        else
            WaveTimingManager.Instance.EnableWaveTimer(true);
    }

    private void OnLayoutUpdateCompleted()
    {
        int nextWaveIndex = WaveTimingManager.Instance.CurrentWaveIndex;
        WaveDetails[] levelWave = WaveTimingManager.Instance.LevelWave;
        EnableNewPortal(levelWave[nextWaveIndex].newPortals);
        WaveTimingManager.Instance.EnableWaveTimer(true);
    }

    public void StartNewWave()
    {
        LevelLayoutManager.Instance.UpdateNavMeshes();
        GiveEnemiesToPortal();
        WaveTimingManager.Instance.EnableWaveTimer(false);
        makingNextWave = false;
    }

    protected void GiveEnemiesToPortal()
    {
        List<string> newEnemyList = GetNewEnemy();
        int portalIndex = 0;

        if (newEnemyList == null) return;

        for (int i = 0; i < newEnemyList.Count; i++)
        {
            string enemyNameToAdd = newEnemyList[i];
            EnemyPortal portalToReceiveEnemy = enemyPortals[portalIndex];

            portalToReceiveEnemy.AddEnemyToList(enemyNameToAdd);
            portalIndex++;

            if (portalIndex >= enemyPortals.Count) portalIndex = 0;
        }
    }

    protected void EnableNewPortal(EnemyPortal[] newPortals)
    {
        foreach (var portal in newPortals)
        {
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }

    protected List<string> GetNewEnemy()
    {
        int nextWaveIndex = WaveTimingManager.Instance.CurrentWaveIndex;
        WaveDetails[] levelWave = WaveTimingManager.Instance.LevelWave;

        if (nextWaveIndex >= levelWave.Length) return null;

        List<string> newEnemyList = new List<string>();

        Array enemyTypes = Enum.GetValues(typeof(EnemyType));
        for (int i = 0; i < enemyTypes.Length; i++)
        {
            EnemyType enemyType = (EnemyType)enemyTypes.GetValue(i);
            if (enemyType == EnemyType.None) continue;

            int count = levelWave[nextWaveIndex].GetEnemyCount(enemyType);
            for (var j = 0; j < count; j++)
                newEnemyList.Add(EnemySpawner.Instance.GetEnemyNameByType(enemyType));
        }

        return newEnemyList;
    }

    protected bool AllEnemiesDefeated()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0 || portal.GetListEnemies().Count > 0) return false;
        }

        return true;
    }

    protected bool HasNewLayout(int nextWaveIndex, WaveDetails[] levelWave) =>
        nextWaveIndex < levelWave.Length && levelWave[nextWaveIndex].nextGrid is not null;
}