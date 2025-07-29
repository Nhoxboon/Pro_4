using System;
using UnityEngine;

public class HideArea : CoreComponent
{
    [SerializeField] protected StealthEnemy enemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemy();
    }

    protected void LoadEnemy()
    {
        if (enemy != null) return;
        enemy = core.GetComponentInParent<StealthEnemy>();
        DebugTool.Log(transform.name + " :LoadEnemy", gameObject);
    }

    protected void OnTriggerEnter(Collider other) => AddEnemyToHideList(other, true);

    protected void OnTriggerExit(Collider other) => AddEnemyToHideList(other, false);

    protected void AddEnemyToHideList(Collider enemyCollider, bool addEnemy)
    {
        Enemy newEnemy = enemyCollider.GetComponent<Enemy>();

        if (newEnemy == null || newEnemy.GetEnemyType() == EnemyType.StealthEnemy) return;
        if (addEnemy)
            enemy.EnemiesToHide.Add(newEnemy);
        else
            enemy.EnemiesToHide.Remove(newEnemy);
    }
}