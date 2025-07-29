using UnityEngine;

public class HeavyEnemyStats : Stats
{
    [SerializeField] protected int shieldValue = 50;

    protected override void Awake()
    {
        base.Awake();

        ShieldAmount.SetMaxValue(shieldValue);
        ShieldAmount.Init();
    }
}