using UnityEngine;
using UnityEngine.EventSystems;

public class BuildBtn : BaseBtn, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Build Button")] [SerializeField]
    protected CameraEffects camEffects;

    [SerializeField] HoverEffect hoverEffect;

    [SerializeField] protected bool btnUnlocked;
    public bool BtnUnlocked => btnUnlocked;

    [Header("Tower Settings")] [SerializeField]
    protected string towerName;

    [SerializeField] protected int towerPrice = 50;
    [SerializeField] protected float towerCenterY = 2f;
    [SerializeField] protected GameObject towerToBuild;
    protected TowerPreview towerPreview;

    [Header("UI Controller")] [SerializeField]
    protected BuildButtonUI buildBtnUI;

    protected override void Start()
    {
        base.Start();
        InitializeTowerPreview();
    }

    protected override void OnClick() => BuildTower();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraEffects();
        LoadHoverEffect();
        LoadBtnUI();
    }

    protected void LoadCameraEffects()
    {
        if (camEffects != null) return;
        camEffects = FindFirstObjectByType<CameraEffects>();
        DebugTool.Log(transform.name + " :LoadCameraEffects", gameObject);
    }

    protected void LoadHoverEffect()
    {
        if (hoverEffect != null) return;
        hoverEffect = GetComponent<HoverEffect>();
        DebugTool.Log(transform.name + " :LoadHoverEffect", gameObject);
    }

    protected void LoadBtnUI()
    {
        if (buildBtnUI != null) return;
        buildBtnUI = transform.GetComponent<BuildButtonUI>();
        DebugTool.Log(transform.name + " :LoadBtnUI", gameObject);
    }

    protected void InitializeTowerPreview()
    {
        if (towerToBuild == null) return;
        towerPreview = TowerPreviewManager.Instance.CreatePreviewForTower(towerToBuild);
    }

    #region Building

    public void BuildTower()
    {
        if (!CanBuild())
        {
            UI.Instance.InGameUI.ShakeCurrencyUI();
            return;
        }

        BuildSlot slotUsed = BuildManager.Instance.SelectedBuildSlot;
        BuildManager.Instance.CancelBuildAction();

        Transform newTower = TowerSpawner.Instance.Spawn(towerName + "Tower",
            slotUsed.GetBuildPosition(towerCenterY), Quaternion.identity);

        if (!ActivateTower(newTower)) return;

        FinalizeSlotAfterBuild(slotUsed);

        UI.Instance.InGameUI.BuildsBtnsUI.SetLastSelectedBtn(null);
        camEffects.ScreenShake(0.15f, 0.2f);
    }

    protected void FinalizeSlotAfterBuild(BuildSlot slot)
    {
        slot.SnapToDefaultPositionImmediately();
        slot.SetSlotAvailable(false);
    }

    #endregion

    #region Button Functionality

    public void UnlockTower(string towerNameChecked, bool unlocked)
    {
        if (towerNameChecked != towerName) return;
        btnUnlocked = unlocked;
        gameObject.SetActive(unlocked);
    }

    public void SelectBtn(bool select)
    {
        hoverEffect?.ShowcaseBtn(select);
        if (towerPreview is null) return;

        BuildSlot slotUsed = BuildManager.Instance.SelectedBuildSlot;
        if (slotUsed is null) return;
        Vector3 previewPosition = slotUsed.GetBuildPosition(towerCenterY);
        TowerPreviewManager.Instance.ShowTowerPreview(towerPreview, select, previewPosition, towerCenterY);

        UI.Instance.InGameUI.BuildsBtnsUI.SetLastSelectedBtn(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildManager.Instance.MouseOverUI(true);
        foreach (var btn in UI.Instance.InGameUI.BuildsBtnsUI.BuildBtns)
            if (btn.gameObject.activeSelf)
                btn.SelectBtn(false);
        SelectBtn(true);
    }

    public void OnPointerExit(PointerEventData eventData) => BuildManager.Instance.MouseOverUI(false);

    #endregion

    protected bool CanBuild() => GameManager.Instance.HasEnoughCurrency(towerPrice) || towerToBuild is null ||
                                 UI.Instance.InGameUI.BuildsBtnsUI.LastSelectedBtn is null;

    protected bool ActivateTower(Transform tower)
    {
        if (tower is null) return false;
        tower.gameObject.SetActive(true);
        return true;
    }

    protected void OnValidate() => buildBtnUI.SetInfo(towerName, towerPrice);
}