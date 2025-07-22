using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class TileManager : NhoxBehaviour
{
    private static TileManager instance;
    public static TileManager Instance => instance;

    [SerializeField] protected float defaultMoveDuration = 0.1f;
    public float DefaultMoveDuration => defaultMoveDuration;

    [Header("Build Slot Movement")] [SerializeField]
    protected float buildSlotYOffset = 0.25f;

    public float BuildSlotYOffset => buildSlotYOffset;

    [Header("Grid Animation Details")] [SerializeField]
    protected float tileMoveDuration = 0.1f;

    [SerializeField] protected float tileDelay = 0.1f;
    [SerializeField] protected float yOffset = 5f;

    [Space] [SerializeField] protected List<GameObject> mainSceneObjects;
    [SerializeField] protected GridBuilder mainSceneGrid;
    protected Coroutine currentActiveCoroutine;
    public Coroutine CurrentActiveCoroutine => currentActiveCoroutine;
    protected bool isGridMoving;
    public bool IsGridMoving => isGridMoving;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one TileManager allowed to exist");
            return;
        }

        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        CollectMainSceneObjects();
        ShowGrid(mainSceneGrid, true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
    }

    protected void LoadGridBuilder()
    {
        if (mainSceneGrid != null) return;
        mainSceneGrid = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + ": LoadGridBuilder", gameObject);
    }

    public void ShowMainGrid(bool showGrid) => ShowGrid(mainSceneGrid, showGrid);

    public void ShowGrid(GridBuilder gridToMove, bool showGrid)
    {
        List<GameObject> objectsToMove = GetObjectsToMove(gridToMove, showGrid);

        if (gridToMove.IsOnFirstBuild())
            ApplyOffset(objectsToMove, new Vector3(0, -yOffset, 0));

        float offset = showGrid ? yOffset : -yOffset;

        currentActiveCoroutine = StartCoroutine(MoveGridCoroutine(objectsToMove, offset));
    }

    private IEnumerator MoveGridCoroutine(List<GameObject> objectsToMove, float yOffs)
    {
        isGridMoving = true;
        for (int i = 0; i < objectsToMove.Count; i++)
        {
            yield return new WaitForSeconds(tileDelay);
            if (objectsToMove[i] is null || !objectsToMove[i]) continue;
            Transform tile = objectsToMove[i].transform;
            MoveTile(tile, tile.position + new Vector3(0, yOffs, 0), tileMoveDuration);
        }

        isGridMoving = false;

    }

    public void MoveTile(Transform objectToMove, Vector3 targetPosition, float? newDuration = null)
    {
        float duration = newDuration ?? defaultMoveDuration;
        StartCoroutine(TileMoveCoroutine(objectToMove, targetPosition, duration));
    }

    public IEnumerator TileMoveCoroutine(Transform objectToMove, Vector3 targetPosition, float? newDuration = null)
    {
        float time = 0f;
        Vector3 startPosition = objectToMove.position;
        float duration = newDuration ?? defaultMoveDuration;

        while (time < duration)
        {
            if (objectToMove.Equals(null)) yield break;
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        if ( !objectToMove.Equals(null))
            objectToMove.position = targetPosition;
    }

    protected void ApplyOffset(List<GameObject> objectsToMove, Vector3 offset)
    {
        foreach (var obj in objectsToMove) obj.transform.position += offset;
    }

    public void EnableMainSceneObjects(bool enable)
    {
        foreach (var obj in mainSceneObjects)
            obj.SetActive(enable);
    }

    protected void CollectMainSceneObjects()
    {
        mainSceneObjects.AddRange(mainSceneGrid.CreatedTiles);
        mainSceneObjects.AddRange(GetExtraObjects());
    }

    protected List<GameObject> GetObjectsToMove(GridBuilder gridToMove, bool starWithTiles)
    {
        List<GameObject> objectsToMove = new List<GameObject>();
        List<GameObject> extraObjects = GetExtraObjects();
        if (starWithTiles)
        {
            objectsToMove.AddRange(gridToMove.CreatedTiles);
            objectsToMove.AddRange(extraObjects);
        }
        else
        {
            objectsToMove.AddRange(extraObjects);
            objectsToMove.AddRange(gridToMove.CreatedTiles);
        }
        return objectsToMove;
    }

    protected List<GameObject> GetExtraObjects()
    {
        List<GameObject> extraObjects = new List<GameObject>();
        extraObjects.AddRange(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.InstanceID)
            .Select(component => component.gameObject));
        extraObjects.AddRange(FindObjectsByType<Castle>(FindObjectsSortMode.InstanceID)
            .Select(component => component.gameObject));
        extraObjects.AddRange(FindObjectsByType<Tower>(FindObjectsSortMode.InstanceID)
            .Select(component => component.gameObject));
        return extraObjects;
    }
}