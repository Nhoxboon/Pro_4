using UnityEngine;

public class BackBtn : CameraEffBtn
{
    protected override void OnClick() => BackToMenu();

    protected void BackToMenu()
    {
        cameraEffects.SwitchToMenuView();
    }
}