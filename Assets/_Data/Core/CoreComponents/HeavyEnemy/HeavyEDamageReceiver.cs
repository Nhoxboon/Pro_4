using UnityEngine;

public class HeavyEDamageReceiver : DamageReceiver
{
    protected override void Awake()
    {
        base.Awake();
        if (core is HeavyEnemyCore heavyCore)
            core.Stats.ShieldAmount.OnCurrentValueZero += heavyCore.ShieldObject.DeactivateShield;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (core is HeavyEnemyCore heavyCore)
            core.Stats.ShieldAmount.OnCurrentValueZero -= heavyCore.ShieldObject.DeactivateShield;
    }

    public override void TakeDamage(float damage)
    {
        if (core.Stats.ShieldAmount.CurrentValue > 0)
        {
            core.Stats.ShieldAmount.Decrease(damage);
            if (core is HeavyEnemyCore heavyCore)
                heavyCore.ShieldObject.ShieldImpact();
        }
        else
            base.TakeDamage(damage);
    }
}