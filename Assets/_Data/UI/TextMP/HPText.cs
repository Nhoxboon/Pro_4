using UnityEngine;

public class HPText : BaseText
{
    protected override void Awake()
    {
        base.Awake();
        ManagerCtrl.Instance.GameManager.OnHPChanged += OnHPChanged;
    }

    protected override void OnDestroy() => ManagerCtrl.Instance.GameManager.OnHPChanged -= OnHPChanged;
    
    protected void OnHPChanged() =>
        UpdateHPui(ManagerCtrl.Instance.GameManager.CurrentHP, ManagerCtrl.Instance.GameManager.MaxHP);

    protected void UpdateHPui(int value, int maxValue)
    {
        int newValue = maxValue - value;
        textMeshPro.text = "Thread: " + newValue + "/" + maxValue;
    }
}