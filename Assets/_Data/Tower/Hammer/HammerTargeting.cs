using UnityEngine;

public class HammerTargeting : TowerTargeting
{
    public bool AtLeastOneTargetInRange()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(towerCtrl.transform.position, attackRange, whatIsEnemy);
        return enemyColliders.Length > 0;
    }
}