public class RestartLevelBtn : BaseBtn
{
    protected override void OnClick() => RestartLevel();

    protected void RestartLevel()
    {
        ManagerCtrl.Instance.LevelManager.RestartLevel();
        ManagerCtrl.Instance.UI.InGameUI.EnableGameOverUI(false);
    }
}