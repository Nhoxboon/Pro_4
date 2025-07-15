using System;
using UnityEngine;

public class BuildManager : NhoxBehaviour
{
    private static BuildManager instance;
    public static BuildManager Instance => instance;

    protected BuildSlot selectedBuildSlot;
    public BuildSlot SelectedBuildSlot => selectedBuildSlot;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one BuildManager allowed to exist");
            return;
        }

        instance = this;
    }

    protected void Update()
    {
        if (InputManager.Instance.IsEscDown) CancelBuildAction();

        if (InputManager.Instance.IsLeftMouseDown)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(InputManager.Instance.MousePosition), out RaycastHit hit))
            {
                if(!hit.collider.TryGetComponent(out BuildSlot _)) CancelBuildAction();
            }
        }
    }

    protected void CancelBuildAction()
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
        if(selectedBuildSlot != null) return;
        UI.Instance.InGameUI.BuildsBtnUI.ShowBtn(true);
    }

    protected void DisableBuildMenu() => UI.Instance.InGameUI.BuildsBtnUI.ShowBtn(false);
}