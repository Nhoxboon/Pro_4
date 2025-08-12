
public class FlyBossDeath : FlyDeath
{
    public override void Die()
    {
        if(core is FlyBossCore flyBossCore)
            flyBossCore.SpawnUnit.EliminateAllUnits();
        base.Die();
    }
}