using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : NhoxBehaviour
{
    [SerializeField] protected List<Waypoint> waypointList;
    public Vector3[] currentWaypoints { get; protected set; }

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
        if(!WaveTimingManager.Instance.GameBegun) return;
        
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && CanSpawn())
        {
            Transform newEnemy = EnemySpawner.Instance.SpawnRandom(enemies, transform.position, Quaternion.identity);
            
            //Or use enemy.SetPortal(this)
            if (newEnemy.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Core.Movement.SetUpEnemy(this);
                PlaceFlyEnemy(newEnemy, enemy.GetEnemyType());
            }
            spawnTimer = spawnCooldown;
            activeEnemies.Add(newEnemy.gameObject);
        }
    }
    
    protected void PlaceFlyEnemy(Transform newEnemy, EnemyType enemyType)
    {
        if(enemyType != EnemyType.FlyingEnemy && enemyType != EnemyType.FlyingBoss) return;
        var flyPosition = transform.position + Vector3.up * 2f;
        
        Transform newFX = FXSpawner.Instance.SpawnParticle("EnemyFlyPortalFX", flyPosition, Quaternion.identity);
        newEnemy.position = flyPosition;
    }

    protected bool CanSpawn() => enemies.Count > 0;
    
    public void AddEnemyToList(string enemyName) => enemies.Add(enemyName);

    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove)) activeEnemies.Remove(enemyToRemove);

        EnemySpawnCoordinator.Instance.HandleWaveCompletion();
    }

    public List<string> GetListEnemies() => enemies;
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

        currentWaypoints = new Vector3[waypointList.Count];

        for (int i = 0; i < currentWaypoints.Length; i++)
            currentWaypoints[i] = waypointList[i].transform.position;
    }
}