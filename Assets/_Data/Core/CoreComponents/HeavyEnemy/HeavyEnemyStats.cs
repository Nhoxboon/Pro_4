using UnityEngine;

public class HeavyEnemyStats : Stats
{
    protected override void Awake()
    {
        base.Awake();

        if (enemyStatsDataSO is not ShieldEnemyDataSO stats) return;
        shieldAmount.SetMaxValue(stats.shield);
        shieldAmount.Init();
    }
}