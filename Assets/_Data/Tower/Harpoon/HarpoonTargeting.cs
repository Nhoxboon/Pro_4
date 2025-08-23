
using UnityEngine;

public class HarpoonTargeting : TowerTargeting
{
    public override void LoseTarget()
    {
        if (towerCtrl.Attack is HarpoonAttack { BusyWithAttack: false })
            base.LoseTarget();
    }

    public override void UpdateTarget()
    {
        if (towerCtrl.Attack is HarpoonAttack { BusyWithAttack: false })
            base.UpdateTarget();
    }
}
