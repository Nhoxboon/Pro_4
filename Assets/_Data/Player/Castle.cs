using System;
using UnityEngine;

public class Castle : NhoxBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy)) enemy.Core.Death.DestroyEnemy();

            GameManager.Instance.UpdateHP(-1);
        }
    }
}