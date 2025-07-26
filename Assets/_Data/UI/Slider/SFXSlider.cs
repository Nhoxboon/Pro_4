using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : AudioSlider
{
    [Header("SFX Settings")] [SerializeField]
    protected string sfxParameter = "sfxVolume";

    protected override void Awake()
    {
        base.Awake();
        audioParameter = sfxParameter;
    }

    protected override void AudioSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }
}