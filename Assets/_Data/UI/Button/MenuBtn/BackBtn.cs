using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : CameraEffBtn
{
    protected override void OnClick() => BackToMenu();

    protected void BackToMenu()
    {
        cameraEffects.SwitchToMenuView();
        ReturnToMenuIfNotThere();
    }

    protected void ReturnToMenuIfNotThere()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == 0) continue;
            LevelManager.Instance.LoadMainMenu();
            break;
        }
    }
}