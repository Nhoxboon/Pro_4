using UnityEngine;

public class SpiderNestCtrl : TowerCtrl
{
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TowerSpawner.Instance.Despawn(gameObject);
        if (!status.IsActive) return;
        attack.DoAttack();
    }

    protected override void LoadTowerRotation()
    {
        // Spider Nest does not rotate, so we skip this method.
    }
}