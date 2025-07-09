using TMPro;
using UnityEngine;

public abstract class SensitiveSlider : BaseSlider
{
    [Header("Sensitive Settings")] [SerializeField]
    protected CameraController cameraController;

    [SerializeField] protected float minSen = 60f;
    [SerializeField] protected float maxSen = 240f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraController();
    }

    protected void LoadCameraController()
    {
        if (cameraController != null) return;
        cameraController = FindFirstObjectByType<CameraController>();
        Debug.Log(transform.name + " :LoadCameraController", gameObject);
    }

    protected override void OnValueChanged(float value) => SetSensitivity(value);

    protected abstract void SetSensitivity(float value);
}