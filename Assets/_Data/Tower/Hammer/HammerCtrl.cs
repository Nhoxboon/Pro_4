using UnityEngine;

public class HammerCtrl : TowerCtrl
{
    protected override void FixedUpdate()
    {
        if (!status.IsActive) return;

        attack.DoAttack();
    }

    protected override void LoadTowerRotation()
    {
        //Not using rotation
    }
}
