using System.Collections.Generic;
using UnityEngine;

public class FlyDeath : Death
{
    protected List<Transform> observingTowers = new();
    protected List<Harpoon> attachedHarpoons = new();

    public void AddObservingTower(Transform tower) => observingTowers.Add(tower);

    public void AddAttachedHarpoon(Harpoon harpoon)
    {
        if (!attachedHarpoons.Contains(harpoon))
            attachedHarpoons.Add(harpoon);
    }

    public override void DestroyEnemy()
    {
        foreach (var harpoon in attachedHarpoons)
            ProjectileSpawner.Instance.Despawn(harpoon.gameObject);

        foreach(var tower in observingTowers)
            if(tower.TryGetComponent(out TowerCtrl towerCtrl) && towerCtrl.Attack is HarpoonAttack attack)
                attack.ResetAttack();

        base.DestroyEnemy();
    }
}
