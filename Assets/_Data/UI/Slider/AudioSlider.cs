using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioSlider : BaseSlider
{
    [Header("Audio Settings")] [SerializeField]
    protected AudioMixer audioMixer;

    [SerializeField] protected float mixerMultiplier = 25f;
    protected string audioParameter;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        slider.value = PlayerPrefs.GetFloat(audioParameter, 0.5f);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerPrefs.SetFloat(audioParameter, slider.value);
    }
    
    protected override void OnValueChanged(float value) => AudioSliderValue(value);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAudioMixer();
    }

    protected void LoadAudioMixer()
    {
        if (audioMixer != null) return;
        audioMixer = Resources.Load<AudioMixer>("Audio/GameMixer");
        DebugTool.Log(transform.name + " :LoadAudioMixer", gameObject);
    }

    protected abstract void AudioSliderValue(float value);
}