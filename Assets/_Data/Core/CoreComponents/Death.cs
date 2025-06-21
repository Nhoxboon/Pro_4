using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    public void Die()
    {
        EnemySpawner.Instance.Despawn(transform.parent.parent.gameObject);
    }
}