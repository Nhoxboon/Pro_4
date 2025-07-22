using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NhoxBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;
    
    protected GridBuilder currentActiveGrid;
    [SerializeField] protected string currentSceneName;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one LevelManager allowed to exist");
            return;
        }

        instance = this;
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            StartCoroutine(LoadLevelCoroutine());
        if (Input.GetKeyDown(KeyCode.K))
            StartCoroutine(LoadMainMenuCoroutine());
    }

    private IEnumerator LoadLevelCoroutine()
    {
        TileManager.Instance.ShowMainGrid(false);
        UI.Instance.EnableMenuUI(false);
        
        yield return TileManager.Instance.CurrentActiveCoroutine;
        TileManager.Instance.EnableMainSceneObjects(false);
        currentSceneName = "Level_1";
        LoadScene("Level_1");
    }
    
    private IEnumerator LoadMainMenuCoroutine()
    {
        EliminateEnemy();
        EliminateAllTowers();
        TileManager.Instance.ShowGrid(currentActiveGrid, false);
        UI.Instance.EnableInGameUI(false);
        
        yield return TileManager.Instance.CurrentActiveCoroutine;
        UnloadCurrentScene();
        TileManager.Instance.EnableMainSceneObjects(true);
        TileManager.Instance.ShowMainGrid(true);
        
        yield return TileManager.Instance.CurrentActiveCoroutine;
        
        UI.Instance.EnableMenuUI(true);
    }

    protected void LoadScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    
    protected void UnloadCurrentScene() => SceneManager.UnloadSceneAsync(currentSceneName);

    protected void EliminateEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            enemy.Core.Death.DestroyEnemy();
        }
    }
    
    protected void EliminateAllTowers()
    {
        Tower[] towers = FindObjectsByType<Tower>(FindObjectsSortMode.None);
        foreach (var tower in towers)
        {
            Destroy(tower.gameObject);
        }
    }
    
    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;
}