using Unity.VisualScripting;
using UnityEngine;

public class TowerPreview : NhoxBehaviour
{
    [SerializeField] protected MeshRenderer[] meshRenderers;
    [SerializeField] protected TowerAtkRadiusDisplay atkRadiusDisplay;
    [SerializeField] protected ForwardAttackDisplay forwardAttackDisplay;
    public ForwardAttackDisplay ForwardAttackDisplay => forwardAttackDisplay;

    protected float attackRange;
    protected bool towerAttackForward;
    
    public void Init(GameObject towerToBuild)
    {
        if(!towerToBuild.TryGetComponent<TowerCtrl>(out var tower)) return;
        
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        GameObject radiusDisplayObj = new GameObject("AttackRadiusDisplay");
        radiusDisplayObj.transform.SetParent(transform);
        atkRadiusDisplay = radiusDisplayObj.AddComponent<TowerAtkRadiusDisplay>();
        forwardAttackDisplay = tower.GetComponentInChildren<ForwardAttackDisplay>();

        attackRange = TowerSpawner.Instance.GetAttackRange(towerToBuild.name);
        towerAttackForward = tower.Status.TowerAttackForward;

        MakeMeshTransparent();

        DestroyExtraComponent(tower);
        gameObject.SetActive(false);
    }

    public void ShowPreview(bool show, Vector3 previewPosition)
    {
        transform.position = previewPosition;
        atkRadiusDisplay.transform.position = new Vector3(previewPosition.x, 0.5f, previewPosition.z);
        
        if(!towerAttackForward)
            atkRadiusDisplay.CreateCircle(show, attackRange);
        else
            forwardAttackDisplay.CreateLines(show, attackRange);
        if (!show) return;
        SetLayerRecursively(gameObject, 0);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        for (int i = 0; i < obj.transform.childCount; i++)
            SetLayerRecursively(obj.transform.GetChild(i).gameObject, newLayer);
    }

    protected void DestroyExtraComponent(TowerCtrl myTower)
    {
        foreach (var component in myTower.Components)
            Destroy(component.gameObject);
        Destroy(myTower);
    }

    protected void MakeMeshTransparent()
    {
        var previewMat = ManagerCtrl.Instance.BuildManager.BuildPreviewMat;

        foreach (var mesh in meshRenderers)
            mesh.material = previewMat;
        if (forwardAttackDisplay == null) return;
        forwardAttackDisplay.ChangeMaterial(previewMat);
    }
}