using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : NhoxBehaviour
{
    [SerializeField] protected WaveDetails currentWave;
    [SerializeField] protected float spawnCooldown = 5f;
    protected float spawnTimer;

    [SerializeField] protected List<string> enemies;

    protected override void Start()
    {
        base.Start();
        enemies = NewEnemyWave();
    }

    protected void Update()
    {
        SpawnEnemy();
    }

    protected void SpawnEnemy()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f && CanSpawn())
        {
            EnemySpawner.Instance.SpawnRandom(enemies, transform.position, Quaternion.identity);
            spawnTimer = spawnCooldown;
        }
    }

    protected bool CanSpawn()
    {
        return enemies.Count > 0;
    }

    protected List<string> NewEnemyWave()
    {
        List<string> newEnemyList = new List<string>();

        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            int count = currentWave.GetEnemyCount(enemyType);
            for (int i = 0; i < count; i++)
            {
                newEnemyList.Add(EnemySpawner.Instance.GetEnemyNameByType(enemyType));
            }
        }

        return newEnemyList;
    }
}