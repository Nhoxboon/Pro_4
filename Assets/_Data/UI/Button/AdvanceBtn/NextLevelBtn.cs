using UnityEngine.SceneManagement;

public class NextLevelBtn : BaseBtn
{
    protected override void OnClick() => NextLevel();

    protected void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
        UI.Instance.InGameUI.EnableLevelCompletedUI(false);
    }
}