using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSetup : NhoxBehaviour
{
    [SerializeField] protected GridBuilder myMainGrid;
    [SerializeField] protected List<GameObject> extraObjectsToDelete;
    [SerializeField] protected Material groundMaterial;

    [Header("Level Details")] [SerializeField]
    protected int levelCurrency = 600;

    [SerializeField] protected TowerUnlockConfigSO towerData;

    protected override void Awake()
    {
        base.Awake();
        ManagerCtrl.Instance?.BuildManager.UpdateBuildManager(WaveTimingManager.Instance, myMainGrid);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetupLevelRoutine());
        UnlockAvailableTowers();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerData();
        LoadGridBuilder();
        LoadObjToDestroy();
        LoadGround();
    }

    protected void LoadTowerData()
    {
        if (towerData != null) return;
        towerData = Resources.Load<TowerUnlockConfigSO>("Tower/" + SceneManager.GetActiveScene().name);
        DebugTool.Log(transform.name + ": LoadTowerData", gameObject);
    }

    protected void LoadGridBuilder()
    {
        if (myMainGrid != null) return;
        myMainGrid = FindFirstObjectByType<GridBuilder>();
        DebugTool.Log(transform.name + ": LoadGridBuilder", gameObject);
    }

    protected void LoadObjToDestroy()
    {
        if (extraObjectsToDelete != null && extraObjectsToDelete.Count > 0) return;
        extraObjectsToDelete = new List<GameObject> { GameObject.Find("---ObjToDestroy---") };
        DebugTool.Log(transform.name + ": LoadObjToDestroy", gameObject);
    }

    protected void LoadGround()
    {
        if (groundMaterial != null) return;
        groundMaterial = GameObject.Find("Ground").GetComponent<MeshRenderer>().sharedMaterial;
        DebugTool.Log(transform.name + ": LoadGround", gameObject);
    }

    protected void UnlockAvailableTowers()
    {
        foreach (var tower in towerData.towerUnlockList)
        {
            foreach (var buildBtn in ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.BuildBtns)
            {
                buildBtn.UnlockTower(tower.towerName, tower.unlocked);
            }
        }

        ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.UpdateUnlockBtn();
    }

    private IEnumerator SetupLevelRoutine()
    {
        if (!LevelWasLoadedToMainScene()) yield break;
        DestroyExtraObjects();

        ManagerCtrl.Instance.LevelManager.UpdateCurrentGrid(myMainGrid);
        ManagerCtrl.Instance.LevelManager.UpdateGroundColor(groundMaterial.color);
        ManagerCtrl.Instance.TileManager.ShowGrid(myMainGrid, true);

        yield return ManagerCtrl.Instance.TileManager.CurrentActiveCoroutine;

        ManagerCtrl.Instance.UI.EnableInGameUI(true);
        ManagerCtrl.Instance.GameManager.PrepareLevel(levelCurrency, WaveTimingManager.Instance);
    }

    protected bool LevelWasLoadedToMainScene() => ManagerCtrl.Instance.LevelManager is not null;

    protected void DestroyExtraObjects()
    {
        foreach (var obj in extraObjectsToDelete) Destroy(obj);
    }
}