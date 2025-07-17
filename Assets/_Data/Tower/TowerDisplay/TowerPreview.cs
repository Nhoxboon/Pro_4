using Unity.VisualScripting;
using UnityEngine;

public class TowerPreview : NhoxBehaviour
{
    [SerializeField] protected MeshRenderer[] meshRenderers;
    protected Tower myTower;
    [SerializeField] protected TowerAtkRadiusDisplay atkRadiusDisplay;

    protected float attackRange;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected void Init()
    {
        myTower = GetComponent<Tower>();
        GameObject radiusDisplayObj = new GameObject("AttackRadiusDisplay");
        radiusDisplayObj.transform.SetParent(transform);
        atkRadiusDisplay = radiusDisplayObj.AddComponent<TowerAtkRadiusDisplay>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        attackRange = TowerSpawner.Instance.GetAttackRange(transform.name.Replace("(Clone)", ""));

        MakeMeshTransparent();

        DestroyExtraComponent();
    }

    public void ShowPreview(bool show, Vector3 previewPosition)
    {
        transform.position = previewPosition;
        atkRadiusDisplay.transform.position = new Vector3(previewPosition.x, 0.5f, previewPosition.z);
        atkRadiusDisplay.CreateCircle(show, attackRange);
    }

    protected void DestroyExtraComponent()
    {
        if (myTower == null) return;
        CrossbowVisual crossbowVisual = myTower.GetComponentInChildren<CrossbowVisual>();
        Destroy(crossbowVisual);
        Destroy(myTower);
    }

    protected void MakeMeshTransparent()
    {
        Material previewMat = FindFirstObjectByType<BuildManager>().BuildPreviewMat;

        foreach (var mesh in meshRenderers)
            mesh.material = previewMat;
    }
}