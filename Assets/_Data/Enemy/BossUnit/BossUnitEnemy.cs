using UnityEngine;

public class BossUnitEnemy : Enemy
{
    protected void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy")) return;
        if(core.Movement is BossUnitMovement bossUnitMovement)
            bossUnitMovement.HandleUnitCollision();
    }
}