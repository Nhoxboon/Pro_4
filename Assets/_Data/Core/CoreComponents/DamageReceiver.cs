public class DamageReceiver : CoreComponent, IDamageable
{
    protected override void Awake()
    {
        base.Awake();
        core.Stats.Health.OnCurrentValueZero += HandleDead;
    }

    protected virtual void OnDestroy() => core.Stats.Health.OnCurrentValueZero -= HandleDead;

    protected void HandleDead()
    {
        if (core.Death.IsDead) return;
        core.Death.SetDead(true);
        core.Death.Die();
    }

    public virtual void TakeDamage(float damage) => core.Stats.Health.Decrease(damage);
}