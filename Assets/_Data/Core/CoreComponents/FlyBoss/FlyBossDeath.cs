using UnityEngine;

public class FlyBossDeath : Death
{
    public override void Die()
    {
        core.SpawnUnit.EliminateAllUnits();
        base.Die();
    }
}