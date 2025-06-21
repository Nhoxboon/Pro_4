using UnityEngine;

public class DamageReceiver : CoreComponent, IDamageable
{
    protected override void Awake()
    {
        base.Awake();
        core.Stats.Health.OnCurrentValueZero += HandleDead;
    }
    
    protected void OnDestroy()
    {
        core.Stats.Health.OnCurrentValueZero -= HandleDead;
    }
    
    protected void HandleDead()
    {
        core.Death.Die();
    }
    
    public void TakeDamage(float damage)
    {
        core.Stats.Health.Decrease(damage);
    }
}