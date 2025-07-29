using UnityEngine.SceneManagement;

public class NextLevelBtn : BaseBtn
{
    protected override void OnClick() => NextLevel();

    protected void NextLevel()
    {
        ManagerCtrl.Instance.LevelManager.LoadNextLevel();
        ManagerCtrl.Instance.UI.InGameUI.EnableLevelCompletedUI(false);
    }
}