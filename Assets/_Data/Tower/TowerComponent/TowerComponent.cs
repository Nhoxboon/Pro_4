using UnityEngine;

public class TowerComponent : NhoxBehaviour
{
    [SerializeField] protected TowerCtrl towerCtrl;

    protected override void Awake()
    {
        base.Awake();
        towerCtrl.AddComponent(this);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTower();
    }

    protected void LoadTower()
    {
        if (towerCtrl != null) return;
        towerCtrl = GetComponentInParent<TowerCtrl>(true);
        DebugTool.Log(transform.name + " :LoadTower", gameObject);
    }
}