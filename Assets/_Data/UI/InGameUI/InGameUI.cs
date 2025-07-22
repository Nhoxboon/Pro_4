using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InGameUI : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI hpText;
    [SerializeField] protected TextMeshProUGUI currencyText;
    [SerializeField] protected TextMeshProUGUI waveTimeText;
    public TextMeshProUGUI WaveTimeText => waveTimeText;

    [SerializeField] protected BuildBtnsUI buildsBtnsUI;
    public BuildBtnsUI BuildsBtnsUI => buildsBtnsUI;

    protected void Update()
    {
        if (InputManager.Instance.IsF10Down) UI.Instance.EnablePauseUI(true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHPText();
        LoadCurrencyText();
        LoadWaveTimeText();
        LoadBuildsBtnsUI();
    }

    protected void LoadHPText()
    {
        if (hpText != null) return;
        hpText = transform.Find("HealthPointUI").GetComponentInChildren<TextMeshProUGUI>(true);
        DebugTool.Log(transform.name + " :LoadHPText", gameObject);
    }

    protected void LoadCurrencyText()
    {
        if (currencyText != null) return;
        currencyText = transform.Find("CurrencyUI").GetComponentInChildren<TextMeshProUGUI>(true);
        DebugTool.Log(transform.name + " :LoadCurrencyText", gameObject);
    }

    protected void LoadWaveTimeText()
    {
        if (waveTimeText != null) return;
        waveTimeText = transform.Find("WaveTimeUI").GetComponentInChildren<TextMeshProUGUI>(true);
        DebugTool.Log(transform.name + " :LoadWaveTimeText", gameObject);
    }

    protected void LoadBuildsBtnsUI()
    {
        if (buildsBtnsUI != null) return;
        buildsBtnsUI = GetComponentInChildren<BuildBtnsUI>(true);
        DebugTool.Log(transform.name + " :LoadBuildsBtnUI", gameObject);
    }

    public void ShakeHPUI() => UI.Instance.UiAnimator.Shake(hpText.transform.parent);
    public void ShakeCurrencyUI() => UI.Instance.UiAnimator.Shake(currencyText.transform.parent);
}