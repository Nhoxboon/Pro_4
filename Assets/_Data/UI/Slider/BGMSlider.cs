using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : AudioSlider
{
    [Header("BGM Settings")] [SerializeField]
    protected string bgmParameter = "backgroundVolume";

    protected override void Awake()
    {
        base.Awake();
        audioParameter = bgmParameter;
    }

    protected override void AudioSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }
}