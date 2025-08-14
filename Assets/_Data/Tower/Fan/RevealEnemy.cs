using System;
using System.Collections.Generic;
using UnityEngine;

public class RevealEnemy : TowerComponent
{
    [SerializeField] protected float revealFrequency = 0.1f;
    [SerializeField] protected float revealDuration = 1f;
    [SerializeField] protected List<Enemy> enemiesToReveal = new();

    protected override void Awake()
    {
        base.Awake();
        InvokeRepeating(nameof(RevealEnemies), 0.1f, revealFrequency);
    }

    protected void RevealEnemies()
    {
        foreach (var enemy in enemiesToReveal)
            enemy.DisableHide(revealDuration);
    }

    public void AddEnemyToReveal(Enemy enemy) => enemiesToReveal.Add(enemy);

    public void RemoveEnemyToReveal(Enemy enemy)
    {
        if (enemiesToReveal.Contains(enemy))
            enemiesToReveal.Remove(enemy);
    }
}