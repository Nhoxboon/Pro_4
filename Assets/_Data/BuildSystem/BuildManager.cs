using System.Collections.Generic;
using UnityEngine;

public class BuildManager : NhoxBehaviour
{
    protected BuildSlot selectedBuildSlot;
    public BuildSlot SelectedBuildSlot => selectedBuildSlot;

    [SerializeField] protected GridBuilder currentGridB;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected CameraEffects camEffects;
    [SerializeField] protected LayerMask whatToIgnore;

    [Header("Build Materials")] 
    [SerializeField] protected Material attackRadMat;
    public Material AttackRadMat => attackRadMat;
    
    [SerializeField] protected Material buildPreviewMat;
    public Material BuildPreviewMat => buildPreviewMat;

    protected bool isMouseOverUI;

    protected override void Awake()
    {
        base.Awake();
        MakeBuildSlotUnavailable(WaveTimingManager.Instance, currentGridB);
    }

    protected void Update()
    {
        if (InputManager.Instance.IsEscDown) CancelBuildAction();

        if (!InputManager.Instance.IsLeftMouseDown || isMouseOverUI) return;
        if (IsClickingNonBuildSlot())
            CancelBuildAction();
    }

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
        LoadMainCamera();
        LoadCameraEffects();
        LoadLayerMask();
        LoadAttackRadMat();
        LoadBuildPreviewMat();
    }

    protected void LoadGridBuilder()
    {
        if (currentGridB != null) return;
        currentGridB = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + " LoadGridBuilder", gameObject);
    }
    
    protected void LoadMainCamera()
    {
        if (mainCamera != null) return;
        mainCamera = Camera.main;
        DebugTool.Log(transform.name + " LoadMainCamera", gameObject);
    }
    
    protected void LoadCameraEffects()
    {
        if (camEffects != null) return;
        camEffects = FindFirstObjectByType<CameraEffects>();
        DebugTool.Log(transform.name + " LoadCameraEffects", gameObject);
    }
    
    protected void LoadLayerMask()
    {
        if (whatToIgnore != 0) return;
        whatToIgnore = LayerMask.GetMask("Default", "FlyRoad", "Untargetable");
        DebugTool.Log(transform.name + " LoadLayerMask", gameObject);
    }
    
    protected void LoadAttackRadMat()
    {
        if (attackRadMat != null) return;
        attackRadMat = Resources.Load<Material>("Materials/AtkRadiusMaterial");
        DebugTool.Log(transform.name + " LoadAttackRadMat", gameObject);
    }
    
    protected void LoadBuildPreviewMat()
    {
        if (buildPreviewMat != null) return;
        buildPreviewMat = Resources.Load<Material>("Materials/BuildPreviewMat");
        DebugTool.Log(transform.name + " LoadBuildPreviewMat", gameObject);
    }
    #endregion

    // public void UpdateBuildManager(WaveTimingManager newWaveManager) => MakeBuildSlotUnavailable(newWaveManager, currentGridB);

    protected bool IsClickingNonBuildSlot()
    {
        return Physics.Raycast(mainCamera.ScreenPointToRay(InputManager.Instance.MousePosition), 
                   out RaycastHit hit, Mathf.Infinity, ~whatToIgnore) 
               && !hit.collider.TryGetComponent<BuildSlot>(out _);
    }

    public void MouseOverUI(bool value) => isMouseOverUI = value;

    public void MakeBuildSlotUnavailable(WaveTimingManager waveManager, GridBuilder currentGrid)
    {
        if (waveManager is null || currentGrid is null) return;
        for (int w = 0; w < waveManager.LevelWave.Length; w++)
        {
            var wave = waveManager.LevelWave[w];
            if (wave.nextGrid is null) continue;

            List<GameObject> grid = currentGrid.CreatedTiles;
            List<GameObject> nextGrid = wave.nextGrid.CreatedTiles;
            // int minCount = Mathf.Min(grid.Count, nextGrid.Count);

            for (int i = 0; i < grid.Count; i++)
            {
                if (!grid[i].TryGetComponent(out TileSlot currentTile)) continue;
                if (!nextGrid[i].TryGetComponent(out TileSlot nextTile)) continue;

                bool tileNotTheSame = currentTile.GetMesh() != nextTile.GetMesh() ||
                                      currentTile.GetMaterial() != nextTile.GetMaterial() ||
                                      currentTile.GetAllChildren().Count != nextTile.GetAllChildren().Count;

                if (!tileNotTheSame) continue;
                if (grid[i].TryGetComponent(out BuildSlot buildSlot))
                    buildSlot.SetSlotAvailable(false);
            }
        }
    }

    public void BuildTower(string towerToBuild, int towerPrice, float towerCenterY, Transform previewTower)
    {
        if (!CanBuild(towerPrice))
        {
            ManagerCtrl.Instance.UI.InGameUI.ShakeCurrencyUI();
            return;
        }

        BuildSlot slotUsed = selectedBuildSlot;
        CancelBuildAction();

        var newTower = TowerSpawner.Instance?.Spawn(towerToBuild,
            slotUsed.GetBuildPosition(towerCenterY), Quaternion.identity);

        if (!ActivateTower(newTower)) return;
        newTower.transform.rotation = previewTower.rotation;
        if(newTower.TryGetComponent(out FanCtrl fanCtrl))
            fanCtrl.ForwardAttackDisplay.UpdateLines();

        FinalizeSlotAfterBuild(slotUsed);

        ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.SetLastSelectedBtn(null, null);
        camEffects.ScreenShake(0.15f, 0.2f);
    }
    
    protected bool CanBuild(int towerPrice) => ManagerCtrl.Instance.GameManager.HasEnoughCurrency(towerPrice) ||
                                 ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.LastSelectedBtn is null;

    protected void FinalizeSlotAfterBuild(BuildSlot slot)
    {
        slot.SnapToDefaultPositionImmediately();
        slot.SetSlotAvailable(false);
    }
    
    protected bool ActivateTower(Transform tower)
    {
        if (tower is null) return false;
        tower.gameObject.SetActive(true);
        return true;
    }

    public void CancelBuildAction()
    {
        if (selectedBuildSlot is null) return;
        ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.LastSelectedBtn?.SelectBtn(false);
        ManagerCtrl.Instance.TowerPreviewManager.HideAllPreviews();

        selectedBuildSlot.UnSelectTile();
        selectedBuildSlot = null;
        DisableBuildMenu();
    }

    public void SelectBuildSlot(BuildSlot buildSlot)
    {
        if (selectedBuildSlot != null) selectedBuildSlot.UnSelectTile();

        selectedBuildSlot = buildSlot;
    }

    public void EnableBuildMenu()
    {
        if (selectedBuildSlot != null) return;
        ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.ShowBtn(true);
    }

    protected void DisableBuildMenu() => ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.ShowBtn(false);
}