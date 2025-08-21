using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelLayoutManager : WaveSystemManager
{
    private static LevelLayoutManager _instance;
    public static LevelLayoutManager Instance => _instance;

    [Header("Level Update")] [SerializeField]
    protected float yOffset = 5f;

    [SerializeField] protected float tileDelay = 0.1f;
    [SerializeField] protected GridBuilder currentGrid;

    [SerializeField] protected NavMeshSurface flyingNavSurface;
    [SerializeField] protected MeshCollider[] flyingNavColliders;
    [SerializeField] protected NavMeshSurface droneNavSurface;

    protected override void SetInstance() => _instance = this;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
        LoadDroneNavMesh();
        LoadFlyNavMesh();
        LoadMeshColliders();
    }

    protected void LoadGridBuilder()
    {
        if (currentGrid != null) return;
        currentGrid = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + ": LoadGridBuilder", gameObject);
    }

    protected void LoadDroneNavMesh()
    {
        if (droneNavSurface != null) return;
        droneNavSurface = GetComponentInChildren<NavMeshSurface>();
        DebugTool.Log(transform.name + ": LoadDroneNavMesh", gameObject);
    }

    protected void LoadFlyNavMesh()
    {
        if (flyingNavSurface != null) return;
        flyingNavSurface = transform.Find("FlyRoad")?.GetComponent<NavMeshSurface>();
        DebugTool.Log(transform.name + ": LoadNavMeshSurface", gameObject);
    }

    protected void LoadMeshColliders()
    {
        if (flyingNavColliders is { Length: > 0 }) return;
        flyingNavColliders = GetComponentsInChildren<MeshCollider>();
        DebugTool.Log(transform.name + ": LoadMeshColliders", gameObject);
    }

    public void UpdateLevelLayout(WaveDetails nextWave, Action onCompleteCallback)
    {
        GridBuilder nextGrid = nextWave.nextGrid;
        List<GameObject> grid = currentGrid.CreatedTiles;
        List<GameObject> newGrid = nextGrid.CreatedTiles;

        if (grid.Count != newGrid.Count) return;

        List<TileSlot> tileToRemove = new List<TileSlot>();
        List<TileSlot> tileToAdd = new List<TileSlot>();

        for (int i = 0; i < grid.Count; i++)
        {
            if (!grid[i].TryGetComponent(out TileSlot currentTile) || !newGrid[i].TryGetComponent(out TileSlot newTile))
                continue;

            if (ShouldUpdateTile(currentTile, newTile))
            {
                tileToRemove.Add(currentTile);
                tileToAdd.Add(newTile);
                grid[i] = newTile.gameObject;
            }
        }

        StartCoroutine(RebuildLevelCoroutine(tileToRemove, tileToAdd, onCompleteCallback, tileDelay));
    }

    private bool ShouldUpdateTile(TileSlot currentTile, TileSlot newTile)
    {
        return currentTile.GetMesh() != newTile.GetMesh() ||
               currentTile.GetMaterial() != newTile.GetMaterial() ||
               currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
               currentTile.transform.rotation != newTile.transform.rotation;
    }

    private IEnumerator RebuildLevelCoroutine(List<TileSlot> tileToRemove, List<TileSlot> tileToAdd,
        Action onCompleteCallback, float delay)
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

        onCompleteCallback?.Invoke();
    }

    protected void AddTile(TileSlot newTile)
    {
        newTile.gameObject.SetActive(true);
        newTile.transform.position += new Vector3(0, -yOffset, 0);
        newTile.transform.parent = currentGrid.transform;

        Vector3 targetPosition = newTile.transform.position + new Vector3(0, yOffset, 0);
        ManagerCtrl.Instance.TileManager.MoveTile(newTile.transform, targetPosition);
    }

    private void RemoveTile(TileSlot tileToRemove)
    {
        Vector3 targetPosition = tileToRemove.transform.position + new Vector3(0, -yOffset, 0);
        ManagerCtrl.Instance.TileManager.MoveTile(tileToRemove.transform, targetPosition);
        Destroy(tileToRemove.gameObject, 1f);
    }

    public void UpdateNavMeshes()
    {
        foreach (var col in flyingNavColliders)
            col.enabled = true;
        flyingNavSurface?.BuildNavMesh();
        foreach (var col in flyingNavColliders)
            col.enabled = false;

        currentGrid.UpdateNavMesh();
        droneNavSurface?.BuildNavMesh();
    }

    public void UpdateDroneNavMesh()
    {
        if (droneNavSurface != null) droneNavSurface.BuildNavMesh();
    }
}