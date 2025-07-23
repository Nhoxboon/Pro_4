using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NhoxBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    protected GridBuilder currentActiveGrid;
    [SerializeField] protected string currentLevelName;
    public string CurrentLevelName => currentLevelName;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            DebugTool.LogError("Only one LevelManager allowed to exist");
            return;
        }

        instance = this;
    }

    //ForTesting purposes
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            LoadLevelFromMenu("Level_1");
        if (Input.GetKeyDown(KeyCode.K))
            LoadMainMenu();
        if (Input.GetKeyDown(KeyCode.R))
            RestartLevel();
    }

    public void RestartLevel() => StartCoroutine(LoadLevelCoroutine(currentLevelName));
    public void LoadLevel(string levelName) => StartCoroutine(LoadLevelCoroutine(levelName));
    public void LoadLevelFromMenu(string levelName) => StartCoroutine(LoadLevelFromMenuCoroutine(levelName));
    public void LoadMainMenu() => StartCoroutine(LoadMainMenuCoroutine());

    private IEnumerator LoadLevelCoroutine(string levelName)
    {
        CleanUpScene();
        UI.Instance.EnableInGameUI(false);
        //Note: Switch camera to game view
        yield return TileManager.Instance.CurrentActiveCoroutine;
        UnloadCurrentScene();
        LoadScene(levelName);
    }

    private IEnumerator LoadLevelFromMenuCoroutine(string levelName)
    {
        TileManager.Instance.ShowMainGrid(false);
        UI.Instance.EnableMenuUI(false);
        //Note: Switch camera to game view
        yield return TileManager.Instance.CurrentActiveCoroutine;
        TileManager.Instance.EnableMainSceneObjects(false);

        LoadScene(levelName);
    }

    protected IEnumerator LoadMainMenuCoroutine()
    {
        CleanUpScene();
        UI.Instance.EnableInGameUI(false);
        //Note: Switch camera to menu view

        yield return TileManager.Instance.CurrentActiveCoroutine;

        UnloadCurrentScene();
        TileManager.Instance.EnableMainSceneObjects(true);
        TileManager.Instance.ShowMainGrid(true);

        yield return TileManager.Instance.CurrentActiveCoroutine;

        UI.Instance.EnableMenuUI(true);
    }

    protected void LoadScene(string sceneName)
    {
        currentLevelName = sceneName;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    protected void UnloadCurrentScene() => SceneManager.UnloadSceneAsync(currentLevelName);

    protected void CleanUpScene()
    {
        WaveTimingManager.Instance?.ResetWaveManager();
        EliminateEnemy();
        EliminateAllTowers();
        if (currentActiveGrid is not null) TileManager.Instance.ShowGrid(currentActiveGrid, false);
    }

    protected void EliminateEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
            enemy.Core.Death.DestroyEnemy();
    }

    protected void EliminateAllTowers()
    {
        Tower[] towers = FindObjectsByType<Tower>(FindObjectsSortMode.None);
        foreach (var tower in towers)
            Destroy(tower.gameObject);
    }

    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;
}