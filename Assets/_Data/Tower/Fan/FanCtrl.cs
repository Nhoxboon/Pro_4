using UnityEngine;

public class FanCtrl : TowerCtrl
{
    [SerializeField] protected RevealEnemy revealEnemy;
    public RevealEnemy RevealEnemy => revealEnemy;

    protected override void Update()
    {
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRevealArea();
    }

    protected void LoadRevealArea()
    {
        if (revealEnemy != null) return;
        revealEnemy = GetComponentInChildren<RevealEnemy>();
        DebugTool.Log(transform.name + " :LoadRevealArea", gameObject);
    }

    protected override void LoadTowerRotation()
    {
    }

    protected override void LoadTowerAttack()
    {
    }

    protected override void LoadTowerVisual()
    {
    }
}