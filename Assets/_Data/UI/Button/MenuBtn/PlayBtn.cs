using UnityEngine;

public class PlayBtn : CameraEffBtn
{
    protected override void OnClick() => Play();

    protected void Play()
    {
        if(ManagerCtrl.Instance.TileManager.IsGridMoving) return;
        cameraEffects.SwitchToLevelSelectView();
    }
}