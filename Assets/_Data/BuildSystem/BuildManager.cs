using System.Collections.Generic;
using UnityEngine;

public class BuildManager : NhoxBehaviour
{
    protected BuildSlot selectedBuildSlot;
    public BuildSlot SelectedBuildSlot => selectedBuildSlot;

    [SerializeField] protected GridBuilder currentGridB;

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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition), out RaycastHit hit))
        {
            if (!hit.collider.TryGetComponent(out BuildSlot _)) CancelBuildAction();
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
        LoadAttackRadMat();
        LoadBuildPreviewMat();
    }

    protected void LoadGridBuilder()
    {
        if (currentGridB != null) return;
        currentGridB = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + " LoadGridBuilder", gameObject);
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

    public void MouseOverUI(bool value) => isMouseOverUI = value;

    public void MakeBuildSlotUnavailable(WaveTimingManager waveManager, GridBuilder currentGrid)
    {
        foreach (var wave in waveManager.LevelWave)
        {
            if (wave.nextGrid == null) continue;
            List<GameObject> grid = currentGrid.CreatedTiles;
            List<GameObject> nextGrid = wave.nextGrid.CreatedTiles;

            for (int i = 0; i < grid.Count; i++)
            {
                TileSlot currentTile = grid[i].GetComponent<TileSlot>();
                TileSlot nextTile = nextGrid[i].GetComponent<TileSlot>();

                bool tileNotTheSame = currentTile.GetMesh() != nextTile.GetMesh() ||
                                      currentTile.GetMaterial() != nextTile.GetMaterial() ||
                                      currentTile.GetAllChildren().Count != nextTile.GetAllChildren().Count;

                if (!tileNotTheSame) continue;
                BuildSlot buildSlot = grid[i].GetComponent<BuildSlot>();
                if (buildSlot != null) buildSlot.SetSlotAvailable(false);
            }
        }
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