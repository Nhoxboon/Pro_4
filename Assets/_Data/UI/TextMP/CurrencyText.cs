using UnityEngine;

public class CurrencyText : BaseText
{
    protected override void Awake()
    {
        base.Awake();
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
    }

    protected override void OnDestroy() => GameManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;

    protected void OnCurrencyChanged() => UpdateCurrencyUI(GameManager.Instance.Currency);

    public void UpdateCurrencyUI(int value) => textMeshPro.text = "Resources: " + value;
}