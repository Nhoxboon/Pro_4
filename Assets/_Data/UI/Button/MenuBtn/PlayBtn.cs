using UnityEngine;

public class PlayBtn : CameraEffBtn
{
    protected override void OnClick() => Play();

    protected void Play()
    {
        if(TileManager.Instance.IsGridMoving) return;
        cameraEffects.SwitchToLevelSelectView();
    }
}