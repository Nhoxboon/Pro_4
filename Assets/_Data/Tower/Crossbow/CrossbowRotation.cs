public class CrossbowRotation : TowerRotation
{
    protected override void LoadTowerHead()
    {
        if (towerHead != null) return;
        towerHead = transform.parent.parent.Find("Model/CrossbowTower/TowerHead");
        DebugTool.Log(transform.name + " :LoadTowerHead", gameObject);
    }
}