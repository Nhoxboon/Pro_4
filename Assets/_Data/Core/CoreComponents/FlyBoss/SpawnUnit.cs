using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : CoreComponent
{
    [SerializeField] protected string bossUnitPrefab;
    [SerializeField] protected int maxUnitCreate = 20;
    protected int amountToCreate;
    [SerializeField] protected float cooldown = 0.05f;
    protected float creationTimer;

    protected List<Enemy> createdEnemies = new List<Enemy>();

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        creationTimer -= Time.deltaTime;

        if (creationTimer < 0 && amountToCreate > 0)
        {
            creationTimer = cooldown;
            CreateNewBossUnit();
        }
    }

    protected void CreateNewBossUnit()
    {
        amountToCreate--;
        Transform newUnit =
            EnemySpawner.Instance.Spawn(bossUnitPrefab, core.Root.transform.position, Quaternion.identity);
        newUnit.gameObject.SetActive(true);

        if (!newUnit.TryGetComponent<BossUnitEnemy>(out var bossEnemy)) return;

        if (bossEnemy.Core.Movement is BossUnitMovement bossMovement)
            bossMovement.SetUpBossUnit(core.Movement.GetFinalWaypoint(), core.Enemy, core.Enemy.MyPortal);
        createdEnemies.Add(bossEnemy);
    }

    public void EliminateAllUnits()
    {
        foreach (var enemy in createdEnemies)
            enemy.Core.Death.Die();
    }

    public void ResetSpawnUnit()
    {
        amountToCreate = maxUnitCreate;
        createdEnemies.Clear();
        creationTimer = 0f;
    }
}