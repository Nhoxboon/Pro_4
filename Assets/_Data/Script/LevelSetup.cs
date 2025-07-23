using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class LevelSetup : NhoxBehaviour
{
    [SerializeField] protected GridBuilder myMainGrid;
    [SerializeField] protected List<GameObject> extraObjectsToDelete;
    [Header("Level Details")]
    [SerializeField] protected int levelCurrency = 600;
    [SerializeField] protected TowerUnlockConfigSO towerData;

    protected override void Start()
    {
        base.Start();
        UnlockAvailableTowers();
        StartCoroutine(SetupLevelRoutine());
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerData();
        LoadGridBuilder();
    }

    protected void LoadTowerData()
    {
        if (towerData != null) return;
        towerData = Resources.Load<TowerUnlockConfigSO>("Tower/TowerOnLevelData");
        DebugTool.Log(transform.name + ": LoadTowerData", gameObject);
    }
    
    protected void LoadGridBuilder()
    {
        if (myMainGrid != null) return;
        myMainGrid = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + ": LoadGridBuilder", gameObject);
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
    
    private IEnumerator SetupLevelRoutine()
    {
        if (!LevelWasLoadedToMainScene()) yield break;
        DestroyExtraObjects();
        
        LevelManager.Instance.UpdateCurrentGrid(myMainGrid);
        TileManager.Instance.ShowGrid(myMainGrid, true);

        yield return TileManager.Instance.CurrentActiveCoroutine;

        UI.Instance.EnableInGameUI(true);
        GameManager.Instance.UpdateGameManager(levelCurrency, WaveTimingManager.Instance);
        WaveTimingManager.Instance.ActivateWaveManager();
    }
    
    protected bool LevelWasLoadedToMainScene()
    {
        return LevelManager.Instance is not null;
    }

    protected void DestroyExtraObjects()
    {
        foreach (var obj in extraObjectsToDelete) DestroyImmediate(obj);
    }
}