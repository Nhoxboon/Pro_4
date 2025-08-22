using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TileManager : NhoxBehaviour
{
    [SerializeField] protected float defaultMoveDuration = 0.05f;
    public float DefaultMoveDuration => defaultMoveDuration;

    [Header("Build Slot Movement")] [SerializeField]
    protected float buildSlotYOffset = 0.25f;

    public float BuildSlotYOffset => buildSlotYOffset;

    [Header("Grid Animation Details")] [SerializeField]
    protected float tileMoveDuration = 0.05f;

    [SerializeField] protected float tileDelay = 0.01f;
    [SerializeField] protected float yOffset = 5f;

    [Space] [SerializeField] protected List<GameObject> mainSceneObjects;
    [SerializeField] protected GridBuilder mainSceneGrid;
    protected Coroutine currentActiveCoroutine;
    public Coroutine CurrentActiveCoroutine => currentActiveCoroutine;
    protected bool isGridMoving;
    public bool IsGridMoving => isGridMoving;

    [Header("Grid Dissolve Details")] [SerializeField]
    protected Material dissolveMaterial;

    [SerializeField] protected float dissolveDuration = 1.2f;
    protected List<Transform> dissolveObj = new();

    protected override void Start()
    {
        base.Start();
        CollectMainSceneObjects();
        if (ManagerCtrl.Instance.GameManager.IsTestingLevel()) return;
        ShowGrid(mainSceneGrid, true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
        LoadDissolveMat();
    }

    protected void LoadGridBuilder()
    {
        if (mainSceneGrid != null) return;
        mainSceneGrid = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + ": LoadGridBuilder", gameObject);
    }

    protected void LoadDissolveMat()
    {
        if (dissolveMaterial != null) return;
        dissolveMaterial = Resources.Load<Material>("Materials/Dissolve_Mat");
        DebugTool.Log(transform.name + ": LoadDissolveMat", gameObject);
    }

    public void ShowMainGrid(bool showGrid) => ShowGrid(mainSceneGrid, showGrid);

    public void ShowGrid(GridBuilder gridToMove, bool showGrid)
    {
        List<GameObject> objectsToMove = GetObjectsToMove(gridToMove, showGrid);

        if (gridToMove.IsOnFirstBuild())
            ApplyOffset(objectsToMove, new Vector3(0, -yOffset, 0));

        float offset = showGrid ? yOffset : -yOffset;

        currentActiveCoroutine = StartCoroutine(MoveGridCoroutine(objectsToMove, offset, showGrid));
    }

    private IEnumerator MoveGridCoroutine(List<GameObject> objectsToMove, float yOffs, bool showGrid)
    {
        isGridMoving = true;
        foreach (var obj in objectsToMove)
        {
            yield return new WaitForSeconds(tileDelay);
            if (!obj) continue;

            Transform tile = obj.transform;
            DissolveTile(showGrid, tile);
            MoveTile(tile, tile.position + new Vector3(0, yOffs, 0), showGrid, tileMoveDuration);
        }

        while (dissolveObj.Count > 0) yield return null;

        isGridMoving = false;
    }

    public void MoveTile(Transform objectToMove, Vector3 targetPosition, bool showGrid, float? newDuration = null)
    {
        float moveDelay = showGrid ? 0f : 0.8f;
        float duration = newDuration ?? defaultMoveDuration;
        StartCoroutine(TileMoveCoroutine(objectToMove, targetPosition, moveDelay, duration));
    }

    public IEnumerator TileMoveCoroutine(Transform objectToMove, Vector3 targetPosition, float delay = 0f,
        float? newDuration = null)
    {
        yield return new WaitForSeconds(delay);

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

        if (!objectToMove.Equals(null))
            objectToMove.position = targetPosition;
    }

    public void DissolveTile(bool showTile, Transform tile)
    {
        if (tile.TryGetComponent(out TileSlot tileSlot))
        {
            for (int i = 0; i < tileSlot.MeshRenderers.Length; i++)
                StartCoroutine(DissolveCoroutine(tileSlot.MeshRenderers[i], dissolveDuration, showTile));
        }
    }

    protected IEnumerator DissolveCoroutine(MeshRenderer meshRenderer, float duration, bool showTile)
    {
        if (meshRenderer.TryGetComponent(out TextMeshPro textMeshPro))
        {
            textMeshPro.enabled = showTile;
            yield break;
        }

        dissolveObj.Add(meshRenderer.transform);

        float startValue = showTile ? 1f : 0f;
        float targetValue = showTile ? 0f : 1f;
        Material originalMat = meshRenderer.material;

        meshRenderer.material = new Material(dissolveMaterial);
        Material dissolveMatInstance = meshRenderer.material;

        dissolveMatInstance.SetColor("_BaseColor", originalMat.GetColor("_BaseColor"));
        dissolveMatInstance.SetFloat("_Metallic", originalMat.GetFloat("_Metallic"));
        dissolveMatInstance.SetFloat("_Smoothness", originalMat.GetFloat("_Smoothness"));
        dissolveMatInstance.SetFloat("_Dissolve", startValue);

        float time = 0f;
        while (time < duration)
        {
            float currentDissolveValue = Mathf.Lerp(startValue, targetValue, time / duration);
            dissolveMatInstance.SetFloat("_Dissolve", currentDissolveValue);
            time += Time.deltaTime;
            yield return null;
        }

        meshRenderer.material = originalMat;
        if (meshRenderer is not null)
            dissolveObj.Remove(meshRenderer.transform);
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
        return extraObjects;
    }
}