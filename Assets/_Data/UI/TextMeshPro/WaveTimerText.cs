
using UnityEngine;

public class WaveTimerText : BaseText
{
    protected override void Start()
    {
        base.Start();
        WaveManager.Instance.OnWaveTimerUpdated += OnWaveTimeChanged;
    }

    protected override void OnDestroy() => WaveManager.Instance.OnWaveTimerUpdated -= OnWaveTimeChanged;
    
    protected void OnWaveTimeChanged()
    {
        UpdateWaveTimeUI(WaveManager.Instance.WaveTimer);
        EnableWaveTimerUI(WaveManager.Instance.WaveTimer > 0);
    }

    public void UpdateWaveTimeUI(float value) => textMeshPro.text = "Seconds: " + value.ToString("00");

    public void EnableWaveTimerUI(bool enable) => textMeshPro.transform.parent.gameObject.SetActive(enable);
}
