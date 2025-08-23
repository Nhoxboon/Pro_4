using System;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : TowerAttack
{
    [Header("Hammer Attack")]
    [Range(0,1)]
    [SerializeField] protected float slowMultiplier = 0.4f;
    [SerializeField] protected float slowDuration;

    protected override bool CanAttack() =>
        Time.time > lastAttackTime + attackCooldown && towerCtrl.Targeting.AtLeastOneTargetInRange();
    
    protected override void Attack()
    {
        base.Attack();
        HammerAttackAnimation();

        foreach (var enemy in ValidEnemyTargets())
            enemy.Core.Movement.SlowEnemy(slowMultiplier, slowDuration);
        AudioManager.Instance.PlaySFX(attackSFX, true);
    }

    protected void HammerAttackAnimation()
    {
        if (towerCtrl.Visuals is HammerVisuals hammerVisual)
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
