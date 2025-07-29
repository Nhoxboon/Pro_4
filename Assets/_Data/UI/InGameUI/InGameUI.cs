using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InGameUI : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI hpText;
    [SerializeField] protected TextMeshProUGUI currencyText;
    [SerializeField] protected TextMeshProUGUI waveTimeText;

    [SerializeField] protected BuildBtnsUI buildsBtnsUI;
    public BuildBtnsUI BuildsBtnsUI => buildsBtnsUI;

    [SerializeField] protected LoadMenuBtn victoryUI;
    [SerializeField] protected RestartLevelBtn gameOverUI;
    [SerializeField] protected NextLevelBtn levelCompletedUI;

    protected void Update()
    {
        if (InputManager.Instance.IsF10Down) ManagerCtrl.Instance.UI.EnablePauseUI(true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHPText();
        LoadCurrencyText();
        LoadWaveTimeText();
        LoadBuildsBtnsUI();
        LoadVictoryUI();
        LoadGameOverUI();
        LoadLevelCompletedUI();
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

    protected void LoadVictoryUI()
    {
        if (victoryUI != null) return;
        victoryUI = GetComponentInChildren<LoadMenuBtn>(true);
        DebugTool.Log(transform.name + " :LoadVictoryUI", gameObject);
    }

    protected void LoadGameOverUI()
    {
        if (gameOverUI != null) return;
        gameOverUI = GetComponentInChildren<RestartLevelBtn>(true);
        DebugTool.Log(transform.name + " :LoadGameOverUI", gameObject);
    }

    protected void LoadLevelCompletedUI()
    {
        if (levelCompletedUI != null) return;
        levelCompletedUI = GetComponentInChildren<NextLevelBtn>(true);
        DebugTool.Log(transform.name + " :LoadLevelCompletedUI", gameObject);
    }

    public void EnableVictoryUI(bool enable) => victoryUI.transform.parent.gameObject.SetActive(enable);
    public void EnableGameOverUI(bool enable) => gameOverUI.transform.parent.gameObject.SetActive(enable);
    public void EnableLevelCompletedUI(bool enable) => levelCompletedUI.transform.parent.gameObject.SetActive(enable);

    public void ShakeHPUI() => ManagerCtrl.Instance.UI.UiAnimator.Shake(hpText.transform.parent);
    public void ShakeCurrencyUI() => ManagerCtrl.Instance.UI.UiAnimator.Shake(currencyText.transform.parent);
}