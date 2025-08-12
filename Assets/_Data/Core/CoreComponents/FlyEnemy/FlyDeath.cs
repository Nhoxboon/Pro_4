using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyDeath : Death
{
    protected List<Transform> observingTowers = new();
    public void AddObservingTower(Transform tower) => observingTowers.Add(tower);

    protected void OnEnable()
    {
        foreach(var tower in observingTowers) 
            if(tower.TryGetComponent(out TowerCtrl towerCtrl) && towerCtrl.Attack is HarpoonAttack attack)
                attack.ResetAttack();
    }
}
