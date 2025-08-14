using UnityEngine;

public class RevealArea : NhoxBehaviour
{
    [SerializeField] protected RevealEnemy revealEnemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRevealEnemy();
    }

    protected void LoadRevealEnemy()
    {
        if (revealEnemy != null) return;
        revealEnemy = GetComponent<RevealEnemy>();
        DebugTool.Log(transform.name + " :LoadRevealEnemy", gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Enemy enemy);
        if (enemy != null)
            revealEnemy.AddEnemyToReveal(enemy);
    }

    protected void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out Enemy enemy);
        if (enemy != null)
            revealEnemy.RemoveEnemyToReveal(enemy);
    }
}