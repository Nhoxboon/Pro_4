using UnityEngine;

public class HeavyEnemyStats : Stats
{
    [SerializeField] protected int shieldValue = 50;

    protected override void Awake()
    {
        base.Awake();

        shieldAmount.SetMaxValue(shieldValue);
        shieldAmount.Init();
    }
}