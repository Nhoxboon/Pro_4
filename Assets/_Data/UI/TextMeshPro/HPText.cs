using UnityEngine;

public class HPText : BaseText
{
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnHPChanged += OnHPChanged;
    }

    protected override void OnDestroy() => GameManager.Instance.OnHPChanged -= OnHPChanged;


    protected void OnHPChanged() => UpdateHPui(GameManager.Instance.CurrentHP, GameManager.Instance.MaxHP);

    protected void UpdateHPui(int value, int maxValue)
    {
        int newValue = maxValue - value;
        textMeshPro.text = "Thread: " + newValue + "/" + maxValue;
    }
}