using UnityEngine;

public class CanonTargeting : TowerTargeting
{
    protected override Enemy FindRandomTargetWithinRange()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(towerCtrl.transform.position, attackRange, whatIsEnemy);

        Enemy bestTarget = null;
        int maxNearbyEnemies = 0;

        foreach (var enemy in enemiesAround)
        {
            int nearbyEnemies = EnemiesAroundEnemy(enemy.transform);

            if (nearbyEnemies <= maxNearbyEnemies) continue;
            maxNearbyEnemies = nearbyEnemies;
            enemy.TryGetComponent(out Enemy enemyComponent);
            bestTarget = enemyComponent;
        }
        return bestTarget;
    }

    protected int EnemiesAroundEnemy(Transform enemyToCheck)
    {
        Collider[] enemiesAround = Physics.OverlapSphere(enemyToCheck.position, 1, whatIsEnemy);

        return enemiesAround.Length;
    }
}