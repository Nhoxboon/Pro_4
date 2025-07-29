using UnityEngine;

public class CurrencyText : BaseText
{
    protected override void Awake()
    {
        base.Awake();
        ManagerCtrl.Instance.GameManager.OnCurrencyChanged += OnCurrencyChanged;
    }

    protected override void OnDestroy() => ManagerCtrl.Instance.GameManager.OnCurrencyChanged -= OnCurrencyChanged;

    protected void OnCurrencyChanged() => UpdateCurrencyUI(ManagerCtrl.Instance.GameManager.Currency);

    public void UpdateCurrencyUI(int value) => textMeshPro.text = "Resources: " + value;
}