using UnityEngine;
using UnityEngine.EventSystems;

public class BuildBtn : BaseBtn, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Build Button")] 
    [SerializeField] HoverEffect hoverEffect;

    [SerializeField] protected bool btnUnlocked;
    public bool BtnUnlocked => btnUnlocked;

    [Header("Tower Settings")] 
    [SerializeField] protected string towerName;

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

    protected override void OnClick() => ConfirmBuildTower();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHoverEffect();
        LoadBtnUI();
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
        towerPreview = ManagerCtrl.Instance.TowerPreviewManager.CreatePreviewForTower(towerToBuild);
    }

    public void ConfirmBuildTower() =>
        ManagerCtrl.Instance.BuildManager.BuildTower(towerToBuild.name, towerPrice, towerCenterY, towerPreview.transform);
    
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

        BuildSlot slotUsed = ManagerCtrl.Instance.BuildManager.SelectedBuildSlot;
        if (slotUsed is null) return;
        Vector3 previewPosition = slotUsed.GetBuildPosition(towerCenterY);
        ManagerCtrl.Instance.TowerPreviewManager.ShowTowerPreview(towerPreview, select, previewPosition,
            towerCenterY + 0.5f);

        ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.SetLastSelectedBtn(this, towerPreview.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ManagerCtrl.Instance.BuildManager.MouseOverUI(true);
        foreach (var btn in ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.BuildBtns)
            if (btn.gameObject.activeSelf)
                btn.SelectBtn(false);
        SelectBtn(true);
    }

    public void OnPointerExit(PointerEventData eventData) => ManagerCtrl.Instance.BuildManager.MouseOverUI(false);

    #endregion

    protected void OnValidate() => buildBtnUI.SetInfo(towerName, towerPrice);
}