using UnityEngine;

public class HammerTargeting : TowerTargeting
{
    public bool AtLeastOneTargetInRange()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy);
        return enemyColliders.Length > 0;
    }
}