using UnityEngine;

public class TowerPreviewManager : NhoxBehaviour
{
    [SerializeField] protected Transform previewHolder;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHolder();
    }

    protected void LoadHolder()
    {
        if (previewHolder != null) return;
        previewHolder = transform.Find("Holder");
        DebugTool.Log(transform.name + " LoadHolder", gameObject);
    }

    public TowerPreview CreatePreviewForTower(GameObject towerPrefab)
    {
        if (towerPrefab == null) return null;

        GameObject previewObject = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity, previewHolder);
        previewObject.name = towerPrefab.name;
        TowerPreview preview = previewObject.AddComponent<TowerPreview>();
        previewObject.SetActive(false);

        return preview;
    }

    public void ShowTowerPreview(TowerPreview preview, bool show, Vector3 position, float towerCenterY)
    {
        if (preview is null) return;

        preview.gameObject.SetActive(show);

        if (show)
            preview.ShowPreview(true, new Vector3(position.x, towerCenterY, position.z));
        else
            preview.ShowPreview(false, Vector3.zero);
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