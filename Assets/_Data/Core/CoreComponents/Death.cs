using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    public virtual void Die()
    {
        DestroyEnemy();
        ManagerCtrl.Instance.GameManager.UpdateCurrency(1);
    }

    public void DestroyEnemy()
    {
        core.Enemy.MyPortal.RemoveActiveEnemy(core.Root.gameObject);

        EnemySpawner.Instance.Despawn(transform.parent.parent.gameObject);
    }
}