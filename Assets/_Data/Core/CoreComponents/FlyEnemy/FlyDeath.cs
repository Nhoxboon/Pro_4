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
        for (int i = 0; i < attachedHarpoons.Count; i++)
            ProjectileSpawner.Instance.Despawn(attachedHarpoons[i].gameObject);

        for (int i = 0; i < observingTowers.Count; i++)
            if (observingTowers[i].TryGetComponent(out TowerCtrl towerCtrl) && towerCtrl.Attack is HarpoonAttack attack)
                attack.ResetAttack();

        base.DestroyEnemy();
    }
}
