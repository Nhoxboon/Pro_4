using UnityEngine;

public class WaveTimerText : BaseText
{
    [SerializeField] protected TextBlinkEffect textEff;
    [SerializeField] protected float waveTimerOffset = 196f;
    protected bool IsEnabled;

    protected override void Start()
    {
        base.Start();
        WaveManager.Instance.OnWaveTimerUpdated += OnWaveTimeChanged;
    }

    protected override void OnDestroy() => WaveManager.Instance.OnWaveTimerUpdated -= OnWaveTimeChanged;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTextBlinkEffect();
    }

    protected void LoadTextBlinkEffect()
    {
        if (textEff != null) return;
        textEff = transform.parent.GetComponentInChildren<TextBlinkEffect>();
        Debug.Log(transform.name + " :LoadTextBlinkEffect", gameObject);
    }

    protected void OnWaveTimeChanged()
    {
        UpdateWaveTimeUI(WaveManager.Instance.WaveTimer);
        EnableWaveTimerUI(WaveManager.Instance.WaveTimer > 0);
    }

    public void UpdateWaveTimeUI(float value) => textMeshPro.text = "Seconds: " + value.ToString("00");

    public void EnableWaveTimerUI(bool enable)
    {
        if (IsEnabled == enable) return;
        IsEnabled = enable;

        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new Vector3(0, yOffset);

        uiAnimator.ChangePos(textMeshPro.transform.parent, offset);
        textEff.EnableBlink(enable);
    }
}