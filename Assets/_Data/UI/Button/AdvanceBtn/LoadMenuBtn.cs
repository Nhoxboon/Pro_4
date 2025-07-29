public class LoadMenuBtn : BaseBtn
{
    protected override void OnClick()
    {
        if (transform.parent.name == "VictoryUI")
            LoadMenuAndDisableVictoryUI();
        else
            LoadMenuAndDisableGameOverUI();
    }

    protected void LoadMenuAndDisableVictoryUI()
    {
        ManagerCtrl.Instance.LevelManager.LoadMainMenu();
        ManagerCtrl.Instance.UI.InGameUI.EnableVictoryUI(false);
    }

    protected void LoadMenuAndDisableGameOverUI()
    {
        ManagerCtrl.Instance.LevelManager.LoadMainMenu();
        ManagerCtrl.Instance.UI.InGameUI.EnableGameOverUI(false);
    }
}