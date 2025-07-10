using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : NhoxBehaviour
{
    private static WaveManager instance;
    public static WaveManager Instance => instance;

    public Action OnWaveTimerUpdated;

    [SerializeField] protected GridBuilder currentGrid;

    protected bool waveCompleted;
    [SerializeField] protected float timeBetweenWaves = 10f;
    [SerializeField] protected float waveTimer;
    public float WaveTimer => waveTimer;

    protected float checkInterval = 0.5f;
    protected float nextCheckTime;

    [SerializeField] protected int nextWaveIndex;
    [SerializeField] protected WaveDetails[] levelWave;

    [SerializeField] protected List<EnemyPortal> enemyPortals;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one instance of WaveManager allowed to exist");
            return;
        }

        instance = this;

        enemyPortals =
            new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
    }

    protected override void Start()
    {
        base.Start();
        SetNextWave();
    }

    protected void Update()
    {
        HandleWaveCompletion();
        UpdateWaveTimer();
    }

    protected void HandleWaveCompletion()
    {
        if (!ReadyToCheck()) return;
        if (waveCompleted || !AllEnemiesDefeated()) return;
        CheckForNewLevelLayout();
        waveCompleted = true;
        waveTimer = timeBetweenWaves;
    }

    protected void UpdateWaveTimer()
    {
        if (!waveCompleted) return;
        waveTimer -= Time.deltaTime;
        OnWaveTimerUpdated?.Invoke();
        if (waveTimer <= 0f) SetNextWave();
    }

    public void ForceNextWave()
    {
        if (!AllEnemiesDefeated()) return;
        waveTimer = 0f;
        OnWaveTimerUpdated?.Invoke();
        SetNextWave();
    }

    [ContextMenu("Set Next Wave")]
    protected void SetNextWave()
    {
        List<string> newEnemyList = NewEnemyWave();
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

        waveCompleted = false;
    }

    protected List<string> NewEnemyWave()
    {
        if (nextWaveIndex >= levelWave.Length) return null;

        List<string> newEnemyList = new List<string>();

        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            if (enemyType == EnemyType.None) continue;

            int count = levelWave[nextWaveIndex].GetEnemyCount(enemyType);
            for (int i = 0; i < count; i++)
            {
                newEnemyList.Add(EnemySpawner.Instance.GetEnemyNameByType(enemyType));
            }
        }

        nextWaveIndex++;

        return newEnemyList;
    }

    protected void CheckForNewLevelLayout()
    {
        if (nextWaveIndex >= levelWave.Length) return;
        WaveDetails newWave = levelWave[nextWaveIndex];
        if (newWave.nextGrid is not null)
        {
            UpdateLevelTiles(newWave.nextGrid);
            EnableNewPortal(newWave.newPortals);
        }

        currentGrid.UpdateNavMesh();
    }

    protected void UpdateLevelTiles(GridBuilder nextGrid)
    {
        List<GameObject> grid = currentGrid.CreatedTiles;
        List<GameObject> newGrid = nextGrid.CreatedTiles;

        //Note: Check if the new grid and current grid have the same number of tiles
        if (grid.Count != newGrid.Count) return;

        for (int i = 0; i < grid.Count; i++)
        {
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot newTile = newGrid[i].GetComponent<TileSlot>();

            bool shouldUpdate = currentTile.GetMesh() != newTile.GetMesh() ||
                                currentTile.GetMaterial() != newTile.GetMaterial() ||
                                currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
                                currentTile.transform.rotation != newTile.transform.rotation;
            if (shouldUpdate)
            {
                currentTile.gameObject.SetActive(false);
                newTile.gameObject.SetActive(true);
                newTile.transform.parent = currentGrid.transform;

                grid[i] = newTile.gameObject;
                Destroy(currentTile.gameObject);
            }
        }
    }

    protected void EnableNewPortal(EnemyPortal[] newPortals)
    {
        foreach (EnemyPortal portal in newPortals)
        {
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }

    protected bool AllEnemiesDefeated()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0) return false;
        }

        return true;
    }

    protected bool ReadyToCheck()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            return true;
        }

        return false;
    }
}