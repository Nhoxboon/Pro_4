using UnityEngine;

public class HPText : BaseText
{
    protected override void Awake()
    {
        base.Awake();
        if (GameManager.Instance != null)
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