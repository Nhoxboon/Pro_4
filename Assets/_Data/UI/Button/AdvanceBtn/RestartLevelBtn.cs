public class RestartLevelBtn : BaseBtn
{
    protected override void OnClick() => RestartLevel();

    protected void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
        UI.Instance.InGameUI.EnableGameOverUI(false);
    }
}