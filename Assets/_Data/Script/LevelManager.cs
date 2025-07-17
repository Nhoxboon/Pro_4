using UnityEngine;

public class LevelManager : NhoxBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [SerializeField] protected TowerUnlockConfigSO towerData;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one LevelManager allowed to exist");
        }
    }

    protected override void Start()
    {
        base.Start();
        UnlockAvailableTowers();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerData();
    }

    protected void LoadTowerData()
    {
        if (towerData != null) return;
        towerData = Resources.Load<TowerUnlockConfigSO>("Tower/TowerOnLevelData");
        Debug.Log(transform.name + ": LoadTowerData", gameObject);
    }

    protected void UnlockAvailableTowers()
    {
        foreach (var tower in towerData.towerUnlockList)
        {
            foreach (var buildBtn in UI.Instance.InGameUI.BuildsBtnsUI.BuildBtns)
            {
                buildBtn.UnlockTower(tower.towerName, tower.unlocked);
            }
        }
        UI.Instance.InGameUI.BuildsBtnsUI.UpdateUnlockBtn();
    }
}