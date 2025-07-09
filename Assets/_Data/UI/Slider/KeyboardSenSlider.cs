using UnityEngine;

public class KeyboardSenSlider : SensitiveSlider
{
    [Header("Keyboard Sen Settings")] protected string keyboardSenPref = "KeyboardSens";

    protected override void OnEnable()
    {
        base.OnEnable();
        slider.value = PlayerPrefs.GetFloat(keyboardSenPref, 0.5f);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerPrefs.SetFloat(keyboardSenPref, slider.value);
    }

    protected override void SetSensitivity(float value)
    {
        var newSen = Mathf.Lerp(minSen, maxSen, value);
        cameraController.AdjustMoveSpeed(newSen);
    }
}