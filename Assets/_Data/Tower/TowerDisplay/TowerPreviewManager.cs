using UnityEngine;

public class TowerPreviewManager : NhoxBehaviour
{
    private static TowerPreviewManager instance;
    public static TowerPreviewManager Instance => instance;

    [SerializeField] protected Transform previewHolder;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one TowerPreviewManager allowed to exist");
            return;
        }
        instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHolder();
    }

    protected void LoadHolder()
    {
        if (previewHolder != null) return;
        previewHolder = transform.Find("Holder");
        Debug.Log(transform.name + " LoadHolder", gameObject);
    }

    public TowerPreview CreatePreviewForTower(GameObject towerPrefab)
    {
        if (towerPrefab == null)
        {
            Debug.LogWarning("Cannot create preview for null tower prefab");
            return null;
        }

        GameObject previewObject = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity, previewHolder);
        TowerPreview preview = previewObject.AddComponent<TowerPreview>();
        previewObject.SetActive(false);

        return preview;
    }

    public void ShowTowerPreview(TowerPreview preview, bool show, Vector3 position, float towerCenterY)
    {
        if (preview is null) return;

        preview.gameObject.SetActive(show);

        if (show)
        {
            Vector3 previewPosition = position;
            if (position.y < towerCenterY)
            {
                previewPosition = new Vector3(position.x, towerCenterY, position.z);
            }

            preview.ShowPreview(true, previewPosition);
        }
        else
        {
            preview.ShowPreview(false, Vector3.zero);
        }
    }

    public void HideAllPreviews()
    {
        for (int i = 0; i < previewHolder.childCount; i++)
        {
            Transform child = previewHolder.GetChild(i);
            if (!child.TryGetComponent<TowerPreview>(out var preview)) continue;
            preview.gameObject.SetActive(false);
            preview.ShowPreview(false, Vector3.zero);
        }
    }
}