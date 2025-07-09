using UnityEngine;

public class MouseSenSlider : SensitiveSlider
{
    [Header("Mouse Sen Settings")] [SerializeField]
    protected string mouseSenPref = "MouseSens";

    protected override void OnEnable()
    {
        base.OnEnable();
        slider.value = PlayerPrefs.GetFloat(mouseSenPref, 0.5f);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerPrefs.SetFloat(mouseSenPref, slider.value);
    }

    protected override void SetSensitivity(float value)
    {
        var newSen = Mathf.Lerp(minSen, maxSen, value);
        cameraController.AdjustMouseSensitivity(newSen);
    }
}