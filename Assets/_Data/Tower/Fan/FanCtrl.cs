using UnityEngine;

public class FanCtrl : TowerCtrl
{
    [SerializeField] protected RevealEnemy revealEnemy;
    public RevealEnemy RevealEnemy => revealEnemy;
    
    [SerializeField] protected ForwardAttackDisplay forwardAttackDisplay;
    public ForwardAttackDisplay ForwardAttackDisplay => forwardAttackDisplay;

    protected override void Update()
    {
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRevealArea();
        LoadForwardAttackDisplay();
    }

    protected void LoadRevealArea()
    {
        if (revealEnemy != null) return;
        revealEnemy = GetComponentInChildren<RevealEnemy>();
        DebugTool.Log(transform.name + " :LoadRevealArea", gameObject);
    }
    
    protected void LoadForwardAttackDisplay()
    {
        if (forwardAttackDisplay != null) return;
        forwardAttackDisplay = GetComponentInChildren<ForwardAttackDisplay>();
        DebugTool.Log(transform.name + " :LoadForwardAttackDisplay", gameObject);
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