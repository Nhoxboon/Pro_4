using UnityEngine;

public class CanonTargeting : TowerTargeting
{
    protected override Enemy FindRandomTargetWithinRange()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(AttackCenter, attackRange, whatIsEnemy);

        Enemy bestTarget = null;
        int maxNearbyEnemies = 0;

        foreach (var enemy in enemiesAround)
        {
            if (!enemy.gameObject.activeInHierarchy) continue;

            if (!enemy.TryGetComponent(out Enemy enemyComponent)) continue;

            int nearbyEnemies = EnemiesAroundEnemy(enemy.transform);

            if (nearbyEnemies <= maxNearbyEnemies) continue;
            maxNearbyEnemies = nearbyEnemies;
            bestTarget = enemyComponent;
        }
        return bestTarget;
    }

    protected int EnemiesAroundEnemy(Transform enemyToCheck)
    {
        Collider[] enemiesAround = Physics.OverlapSphere(enemyToCheck.position, 1, whatIsEnemy);
        int activeCount = 0;

        foreach (var col in enemiesAround)
        {
            if (col.gameObject.activeInHierarchy)
                activeCount++;
        }

        return activeCount;
    }
}