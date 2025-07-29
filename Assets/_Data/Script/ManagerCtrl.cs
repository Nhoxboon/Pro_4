using UnityEngine;

public class ManagerCtrl : NhoxBehaviour
{
    private static ManagerCtrl instance;
    public static ManagerCtrl Instance => instance ??= FindFirstObjectByType<ManagerCtrl>(); //Note: Dangerous

    [SerializeField] protected GameManager gameManager;
    public GameManager GameManager => gameManager;

    [SerializeField] protected TileManager tileManager;
    public TileManager TileManager => tileManager;

    [SerializeField] protected LevelManager levelManager;
    public LevelManager LevelManager => levelManager;

    [SerializeField] protected BuildManager buildManager;
    public BuildManager BuildManager => buildManager;

    [SerializeField] protected TowerPreviewManager towerPreviewManager;
    public TowerPreviewManager TowerPreviewManager => towerPreviewManager;

    [SerializeField] protected UI ui;
    public UI UI => ui;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one ManagerCtrl allowed to exist");
            return;
        }

        instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGameManager();
        LoadTileManager();
        LoadLevelManager();
        LoadBuildManager();
        LoadTowerPreviewManager();
        LoadUI();
    }

    protected void LoadGameManager()
    {
        if (gameManager != null) return;
        gameManager = GetComponentInChildren<GameManager>();
        DebugTool.Log(transform.name + " :LoadGameManager", gameObject);
    }

    protected void LoadTileManager()
    {
        if (tileManager != null) return;
        tileManager = GetComponentInChildren<TileManager>();
        DebugTool.Log(transform.name + " :LoadTileManager", gameObject);
    }

    protected void LoadLevelManager()
    {
        if (levelManager != null) return;
        levelManager = GetComponentInChildren<LevelManager>();
        DebugTool.Log(transform.name + " :LoadLevelManager", gameObject);
    }

    protected void LoadBuildManager()
    {
        if (buildManager != null) return;
        buildManager = GetComponentInChildren<BuildManager>();
        DebugTool.Log(transform.name + " :LoadBuildManager", gameObject);
    }

    protected void LoadTowerPreviewManager()
    {
        if (towerPreviewManager != null) return;
        towerPreviewManager = GetComponentInChildren<TowerPreviewManager>();
        DebugTool.Log(transform.name + " :LoadTowerPreviewManager", gameObject);
    }

    protected void LoadUI()
    {
        if (ui != null) return;
        ui = FindFirstObjectByType<UI>();
        DebugTool.Log(transform.name + " :LoadUI", gameObject);
    }
}