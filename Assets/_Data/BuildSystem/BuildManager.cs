using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : NhoxBehaviour
{
    private static BuildManager instance;
    public static BuildManager Instance => instance;

    protected BuildSlot selectedBuildSlot;
    public BuildSlot SelectedBuildSlot => selectedBuildSlot;

    [SerializeField] protected GridBuilder currentGridB;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one BuildManager allowed to exist");
            return;
        }

        instance = this;
        MakeBuildSlotUnavailable(WaveManager.Instance, currentGridB);
    }

    protected void Update()
    {
        if (InputManager.Instance.IsEscDown) CancelBuildAction();

        if (InputManager.Instance.IsLeftMouseDown)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition), out RaycastHit hit))
            {
                if (!hit.collider.TryGetComponent(out BuildSlot _)) CancelBuildAction();
            }
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGridBuilder();
    }

    protected void LoadGridBuilder()
    {
        if (currentGridB != null) return;
        currentGridB = FindFirstObjectByType<GridBuilder>();
        Debug.Log(transform.name + " LoadGridBuilder", gameObject);
    }

    public void MakeBuildSlotUnavailable(WaveManager waveManager, GridBuilder currentGrid)
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
        UI.Instance.InGameUI.BuildsBtnsUI.ShowBtn(true);
    }

    protected void DisableBuildMenu() => UI.Instance.InGameUI.BuildsBtnsUI.ShowBtn(false);
}