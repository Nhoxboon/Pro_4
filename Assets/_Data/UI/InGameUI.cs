using System;
using TMPro;
using UnityEngine;

public class InGameUI : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI hpText;
    [SerializeField] protected TextMeshProUGUI currencyText;
    [SerializeField] protected TextMeshProUGUI waveTimeText;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHPText();
        LoadCurrencyText();
        LoadWaveTimeText();
    }

    protected void LoadHPText()
    {
        if (hpText != null) return;
        hpText = transform.Find("HealthPointUI").GetComponentInChildren<TextMeshProUGUI>(true);
        Debug.Log(transform.name + " :LoadHPText", gameObject);
    }

    protected void LoadCurrencyText()
    {
        if (currencyText != null) return;
        currencyText = transform.Find("CurrencyUI").GetComponentInChildren<TextMeshProUGUI>(true);
        Debug.Log(transform.name + " :LoadCurrencyText", gameObject);
    }

    protected void LoadWaveTimeText()
    {
        if (waveTimeText != null) return;
        waveTimeText = transform.Find("WaveTimeUI").GetComponentInChildren<TextMeshProUGUI>(true);
        Debug.Log(transform.name + " :LoadWaveTimeText", gameObject);
    }
}