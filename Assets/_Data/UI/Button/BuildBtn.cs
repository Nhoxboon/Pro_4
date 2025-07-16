using TMPro;
using UnityEngine;

public class BuildBtn : BaseBtn
{
    [Header("Build Button")] [SerializeField]
    protected CameraEffects camEffects;

    [Header("Tower Settings")] [SerializeField]
    protected string towerName;

    [SerializeField] protected int towerPrice = 50;
    [SerializeField] protected float towerCenterY = 2f;

    [Header("UI Controller")] [SerializeField]
    protected BuildButtonUI buildBtnUI;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraEffects();
        LoadBtnUI();
    }

    protected override void OnClick() => BuildTower();

    protected void LoadCameraEffects()
    {
        if (camEffects != null) return;
        camEffects = FindFirstObjectByType<CameraEffects>();
        Debug.Log(transform.name + " :LoadCameraEffects", gameObject);
    }

    protected void LoadBtnUI()
    {
        if (buildBtnUI != null) return;
        buildBtnUI = transform.GetComponent<BuildButtonUI>();
        Debug.Log(transform.name + " :LoadBtnUI", gameObject);
    }

    public void UnlockTower(string towerNameChecked, bool unlocked)
    {
        if (towerNameChecked != towerName) return;

        gameObject.SetActive(unlocked);
    }

    public void BuildTower()
    {
        if (!CanBuild()) return;

        BuildSlot slotUsed = BuildManager.Instance.SelectedBuildSlot;
        BuildManager.Instance.CancelBuildAction();

        Transform newTower = TowerSpawner.Instance.Spawn(towerName + "Tower",
            slotUsed.GetBuildPosition(towerCenterY),
            Quaternion.identity);

        if (!ActivateTower(newTower)) return;

        slotUsed.SnapToDefaultPositionImmediately();
        slotUsed.SetSlotAvailable(false);

        camEffects.ScreenShake(0.15f, 0.2f);
    }

    protected bool CanBuild()
    {
        return GameManager.Instance.HasEnoughCurrency(towerPrice);
    }

    protected bool ActivateTower(Transform tower)
    {
        if (tower == null) return false;
        tower.gameObject.SetActive(true);
        return true;
    }

    protected void OnValidate() => buildBtnUI.SetInfo(towerName, towerPrice);
}