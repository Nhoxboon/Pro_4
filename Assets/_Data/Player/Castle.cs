
using System;
using UnityEngine;

public class Castle : NhoxBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemySpawner.Instance.Despawn(other.gameObject);
        }
    }
}
