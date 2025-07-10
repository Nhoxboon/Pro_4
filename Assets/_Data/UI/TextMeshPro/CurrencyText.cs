
using UnityEngine;

public class CurrencyText : BaseText
{
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
    }

    protected override void OnDestroy() => GameManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;
    
    protected void OnCurrencyChanged() => UpdateCurrencyUI(GameManager.Instance.Currency);
    
    public void UpdateCurrencyUI(int value) => textMeshPro.text = "Resources: " + value;
    
}
