using UnityEngine;

public abstract class CameraEffBtn : BaseBtn
{
    [SerializeField] protected CameraEffects cameraEffects;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraEffects();
    }

    protected void LoadCameraEffects()
    {
        if (cameraEffects != null) return;
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        DebugTool.Log(transform.name + " :LoadCameraEffects", gameObject);
    }
}