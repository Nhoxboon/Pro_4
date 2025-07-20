using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: Trash but not delete yet, this is the old WaveManager that was split into multiple classes
public class WaveManager : NhoxBehaviour
{
    private static WaveManager instance;
    public static WaveManager Instance => instance;

    public Action OnWaveTimerUpdated;

    [SerializeField] protected GridBuilder currentGrid;
    public bool gameBegun;

    [Header("Wave Details")] [SerializeField]
    protected float timeBetweenWaves = 10f;

    [SerializeField] protected float waveTimer;
    public float WaveTimer => waveTimer;
    [SerializeField] protected int nextWaveIndex;
    [SerializeField] protected WaveDetails[] levelWave;
    public WaveDetails[] LevelWave => levelWave;
    protected bool waveTimerEnabled;
    public bool WaveTimerEnabled => waveTimerEnabled;
    protected bool makingNextWave;

    [Header("Level Update")] protected float yOffset = 5;
    protected float tileDelay = 0.1f;
    [SerializeField] protected List<EnemyPortal> enemyPortals;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one WaveManager allowed to exist");
            return;
        }

        instance = this;

        enemyPortals =
            new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
    }

    protected void Update()
    {
        if (!gameBegun) return;
        UpdateWaveTimer();
    }

    [ContextMenu("Activate Wave Manager")]
    public void ActivateWaveManager()
    {
        gameBegun = true;
        EnableWaveTimer(true);
    }

    public void HandleWaveCompletion()
    {
        if (!AllEnemiesDefeated() || makingNextWave) return;
        makingNextWave = true;
        nextWaveIndex++;

        if (HasNoMoreWaves()) return;

        if (HasNewLayout())
            UpdateLevelLayout(levelWave[nextWaveIndex]);
        else
            EnableWaveTimer(true);
    }

    public void StartNewWave()
    {
        currentGrid.UpdateNavMesh();
        GiveEnemiesToPortal();
        EnableWaveTimer(false);
        makingNextWave = false;
    }

    protected void UpdateWaveTimer()
    {
        if (!waveTimerEnabled) return;
        waveTimer -= Time.deltaTime;
        OnWaveTimerUpdated?.Invoke();
        if (waveTimer <= 0f) StartNewWave();
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

    protected void UpdateLevelLayout(WaveDetails nextWave)
    {
        GridBuilder nextGrid = nextWave.nextGrid;
        List<GameObject> grid = currentGrid.CreatedTiles;
        List<GameObject> newGrid = nextGrid.CreatedTiles;

        //Note: Check if the new grid and current grid have the same number of tiles
        if (grid.Count != newGrid.Count) return;

        List<TileSlot> tileToRemove = new List<TileSlot>();
        List<TileSlot> tileToAdd = new List<TileSlot>();

        for (int i = 0; i < grid.Count; i++)
        {
            if (!grid[i].TryGetComponent(out TileSlot currentTile) || !newGrid[i].TryGetComponent(out TileSlot newTile))
                continue;

            bool shouldUpdate = currentTile.GetMesh() != newTile.GetMesh() ||
                                currentTile.GetMaterial() != newTile.GetMaterial() ||
                                currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
                                currentTile.transform.rotation != newTile.transform.rotation;
            if (!shouldUpdate) continue;
            tileToRemove.Add(currentTile);
            tileToAdd.Add(newTile);

            grid[i] = newTile.gameObject;
        }

        StartCoroutine(RebuildLevelCoroutine(tileToRemove, tileToAdd, nextWave, tileDelay));
    }

    private IEnumerator RebuildLevelCoroutine(List<TileSlot> tileToRemove, List<TileSlot> tileToAdd,
        WaveDetails waveDetails, float delay)
    {
        foreach (var t in tileToRemove)
        {
            yield return new WaitForSeconds(delay);
            RemoveTile(t);
        }

        foreach (var t in tileToAdd)
        {
            yield return new WaitForSeconds(delay);
            AddTile(t);
        }

        EnableNewPortal(waveDetails.newPortals);
        EnableWaveTimer(true);
    }

    protected void AddTile(TileSlot newTile)
    {
        newTile.gameObject.SetActive(true);
        newTile.transform.position += new Vector3(0, -yOffset, 0);
        newTile.transform.parent = currentGrid.transform;

        Vector3 targetPosition = newTile.transform.position + new Vector3(0, yOffset, 0);
        TileManager.Instance.MoveTile(newTile.transform, targetPosition);
    }

    protected void RemoveTile(TileSlot tileToRemove)
    {
        Vector3 targetPosition = tileToRemove.transform.position + new Vector3(0, -yOffset, 0);
        TileManager.Instance.MoveTile(tileToRemove.transform, targetPosition);
        Destroy(tileToRemove.gameObject, 1);
    }

    protected void EnableWaveTimer(bool enable)
    {
        if (waveTimerEnabled == enable) return;

        waveTimerEnabled = enable;
        waveTimer = timeBetweenWaves;
        OnWaveTimerUpdated?.Invoke();
    }

    protected void EnableNewPortal(EnemyPortal[] newPortals)
    {
        for (int i = 0; i < newPortals.Length; i++)
        {
            EnemyPortal portal = newPortals[i];
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }

    protected List<string> GetNewEnemy()
    {
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
            if (portal.GetActiveEnemies().Count > 0) return false;
        }

        return true;
    }

    protected bool HasNewLayout() => nextWaveIndex < levelWave.Length && levelWave[nextWaveIndex].nextGrid is not null;

    protected bool HasNoMoreWaves() => nextWaveIndex >= levelWave.Length;
}