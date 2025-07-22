using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one instance of EnemySpawner allow to exist");
            return;
        }

        instance = this;
    }

    public string GetEnemyNameByType(EnemyType enemyType)
    {
        return enemyType.ToString();
    }

    public Transform SpawnRandom(List<string> enemyPool, Vector3 spawnPos, Quaternion rotation)
    {
        if (enemyPool == null || enemyPool.Count == 0) return null;

        int randomIndex = Random.Range(0, enemyPool.Count);
        string enemyToSpawn = enemyPool[randomIndex];
        enemyPool.RemoveAt(randomIndex);

        Transform newEnemy = Spawn(enemyToSpawn, spawnPos, rotation);

        newEnemy.gameObject.SetActive(true);

        return newEnemy;
    }
}