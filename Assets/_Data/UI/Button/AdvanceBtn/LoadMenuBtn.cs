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
        LevelManager.Instance.LoadMainMenu();
        UI.Instance.InGameUI.EnableVictoryUI(false);
    }

    protected void LoadMenuAndDisableGameOverUI()
    {
        LevelManager.Instance.LoadMainMenu();
        UI.Instance.InGameUI.EnableGameOverUI(false);
    }
}