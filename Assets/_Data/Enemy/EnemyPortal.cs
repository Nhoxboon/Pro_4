using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : NhoxBehaviour
{
    [SerializeField] protected List<Waypoint> waypointList;
    [SerializeField] protected float spawnCooldown = 5f;
    protected float spawnTimer;

    [SerializeField] protected List<string> enemies = new();
    [SerializeField] protected List<GameObject> activeEnemies = new();

    protected override void Awake()
    {
        base.Awake();
        CollectWaypoints();
    }

    protected void Update() => SpawnEnemy();

    protected void SpawnEnemy()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f && CanSpawn())
        {
            Transform newEnemy = EnemySpawner.Instance.SpawnRandom(enemies, transform.position, Quaternion.identity);

            //Or use enemy.SetPortal(this)
            if (newEnemy.TryGetComponent<Enemy>(out var enemy)) enemy.Core.Movement.SetUpEnemy(waypointList, this);

            spawnTimer = spawnCooldown;
            activeEnemies.Add(newEnemy.gameObject);
        }
    }

    protected bool CanSpawn()
    {
        return enemies.Count > 0;
    }

    public void AddEnemyToList(string enemyName) => enemies.Add(enemyName);

    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove)) activeEnemies.Remove(enemyToRemove);

        EnemySpawnCoordinator.Instance.HandleWaveCompletion();
    }

    public List<GameObject> GetActiveEnemies() => activeEnemies;

    [ContextMenu("Collect Waypoints")]
    protected void CollectWaypoints()
    {
        waypointList = new List<Waypoint>();
        foreach (Transform child in transform.Find("Waypoints"))
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint != null) waypointList.Add(waypoint);
        }
    }
}