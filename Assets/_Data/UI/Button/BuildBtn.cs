using UnityEngine;
using UnityEngine.EventSystems;

public class BuildBtn : BaseBtn, IPointerEnterHandler
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
        Debug.Log(transform.name + " :LoadCameraEffects", gameObject);
    }

    protected void LoadHoverEffect()
    {
        if(hoverEffect != null) return;
        hoverEffect = GetComponent<HoverEffect>();
        Debug.Log(transform.name + " :LoadHoverEffect", gameObject);
    }

    protected void LoadBtnUI()
    {
        if (buildBtnUI != null) return;
        buildBtnUI = transform.GetComponent<BuildButtonUI>();
        Debug.Log(transform.name + " :LoadBtnUI", gameObject);
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
        foreach (var btn in UI.Instance.InGameUI.BuildsBtnsUI.BuildBtns)
            btn.SelectBtn(false);
        SelectBtn(true);
    }
    #endregion

    protected bool CanBuild() => GameManager.Instance.HasEnoughCurrency(towerPrice);

    protected bool ActivateTower(Transform tower)
    {
        if (tower == null) return false;
        tower.gameObject.SetActive(true);
        return true;
    }

    protected void OnValidate() => buildBtnUI.SetInfo(towerName, towerPrice);
}