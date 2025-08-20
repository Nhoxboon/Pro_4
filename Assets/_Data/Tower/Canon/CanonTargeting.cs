using UnityEngine;

public class CanonTargeting : TowerTargeting
{
    protected override Enemy FindRandomTargetWithinRange()
    {
        int colFound = Physics.OverlapSphereNonAlloc(AttackCenter, attackRange, allocatedColliders, whatIsEnemy);

        Enemy bestTarget = null;
        int maxNearbyEnemies = 0;

        for (int i = 0; i < colFound; i++)
        {
            Transform enemy = allocatedColliders[i].transform;
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
        int enemiesCount = Physics.OverlapSphereNonAlloc(enemyToCheck.position, 1, allocatedColliders, whatIsEnemy);
        int activeCount = 0;

        for (int i = 0; i < enemiesCount; i++)
        {
            if (allocatedColliders[i].gameObject.activeInHierarchy)
                activeCount++;
        }
        return activeCount;
    }
}