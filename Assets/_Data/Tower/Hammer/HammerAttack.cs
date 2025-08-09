using System;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : TowerAttack
{
    [Header("Hammer Attack")]
    [Range(0,1)]
    [SerializeField] protected float slowMultiplier = 0.4f;
    [SerializeField] protected float slowDuration;

    protected override bool CanAttack()
    {
        if(towerCtrl.Targeting is HammerTargeting hammerTargeting)
            return Time.time > lastAttackTime + attackCooldown
                   && hammerTargeting.AtLeastOneTargetInRange();

        return base.CanAttack();
    }

    protected override void Attack()
    {
        base.Attack();
        HammerAttackAnimation();

        foreach (var enemy in ValidEnemyTargets())
            enemy.Core.Movement.SlowEnemy(slowMultiplier, slowDuration);
    }

    protected void HammerAttackAnimation()
    {
        if (towerCtrl.Visual is HammerVisual hammerVisual)
            hammerVisual.HammerAttackVFX();
    }

    protected List<Enemy> ValidEnemyTargets()
    {
        List<Enemy> targets = new List<Enemy>();
        Collider[] enemiesAround =
            Physics.OverlapSphere(towerCtrl.Targeting.AttackCenter, towerCtrl.Targeting.AttackRange, whatIsTargetable);
        for (int i = 0; i < enemiesAround.Length; i++)
        {
            if(enemiesAround[i].TryGetComponent(out Enemy enemy))
                targets.Add(enemy);
        }
        return targets;
    }
}
