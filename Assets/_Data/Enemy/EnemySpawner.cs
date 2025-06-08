using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : NhoxBehaviour
{
    //Note: Just for testing purposes
    [SerializeField] protected WaveDetails currentWave;
    [SerializeField] protected float spawnCooldown = 5f;
    protected float spawnTimer;

    [SerializeField] protected List<GameObject> enemies;
    [SerializeField] protected GameObject basicEnemyPrefab;
    [SerializeField] protected GameObject fastEnemyPrefab;

    protected override void Start()
    {
        base.Start();

        enemies = NewEnemyWave();
    }

    protected void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f && enemies.Count > 0)
        {
            SpawnEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    protected void SpawnEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);
    }
    
    protected GameObject GetRandomEnemy()
    {
        if (enemies.Count == 0) return null;

        int randomIndex = Random.Range(0, enemies.Count);
        GameObject enemyToSpawn = enemies[randomIndex];
        enemies.RemoveAt(randomIndex);

        return enemyToSpawn;
    }

    protected List<GameObject> NewEnemyWave()
    {
        List<GameObject> newEnemyList = new List<GameObject>();
        
        for (int i = 0; i < currentWave.basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemyPrefab);
        }
        
        for (int i = 0; i < currentWave.fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemyPrefab);
        }
        return newEnemyList;
    }
}