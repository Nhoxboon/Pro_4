using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NhoxBehaviour
{
    protected GridBuilder currentActiveGrid;
    [SerializeField] protected string currentLevelName;
    public string CurrentLevelName => currentLevelName;
    [SerializeField] protected CameraEffects cameraEffects;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraEffects();
    }

    protected void LoadCameraEffects()
    {
        if (cameraEffects != null) return;
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        DebugTool.Log(transform.name + " :LoadCameraEffects", gameObject);
    }

    public void RestartLevel() => StartCoroutine(LoadLevelCoroutine(currentLevelName));
    public void LoadLevel(string levelName) => StartCoroutine(LoadLevelCoroutine(levelName));
    public void LoadNextLevel() => LoadLevel(GetNextLevelName());
    public void LoadLevelFromMenu(string levelName) => StartCoroutine(LoadLevelFromMenuCoroutine(levelName));
    public void LoadMainMenu() => StartCoroutine(LoadMainMenuCoroutine());

    private IEnumerator LoadLevelCoroutine(string levelName)
    {
        CleanUpScene();
        ManagerCtrl.Instance.UI.EnableInGameUI(false);
        cameraEffects.SwitchToGameView();

        yield return ManagerCtrl.Instance.TileManager.CurrentActiveCoroutine;
        UnloadCurrentScene();
        LoadScene(levelName);
    }

    private IEnumerator LoadLevelFromMenuCoroutine(string levelName)
    {
        ManagerCtrl.Instance.TileManager.ShowMainGrid(false);
        ManagerCtrl.Instance.UI.EnableMenuUI(false);
        cameraEffects.SwitchToGameView();

        yield return ManagerCtrl.Instance.TileManager.CurrentActiveCoroutine;
        ManagerCtrl.Instance.TileManager.EnableMainSceneObjects(false);

        LoadScene(levelName);
    }

    protected IEnumerator LoadMainMenuCoroutine()
    {
        CleanUpScene();
        ManagerCtrl.Instance.UI.EnableInGameUI(false);
        cameraEffects.SwitchToMenuView();

        yield return ManagerCtrl.Instance.TileManager.CurrentActiveCoroutine;

        UnloadCurrentScene();
        ManagerCtrl.Instance.TileManager.EnableMainSceneObjects(true);
        ManagerCtrl.Instance.TileManager.ShowMainGrid(true);

        yield return ManagerCtrl.Instance.TileManager.CurrentActiveCoroutine;

        ManagerCtrl.Instance.UI.EnableMenuUI(true);
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
        if (currentActiveGrid is not null) ManagerCtrl.Instance.TileManager.ShowGrid(currentActiveGrid, false);
    }

    protected void EliminateEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
            enemy.Core.Death.DestroyEnemy();
    }

    protected void EliminateAllTowers()
    {
        TowerCtrl[] towers = FindObjectsByType<TowerCtrl>(FindObjectsSortMode.None);
        foreach (var tower in towers)
            tower.Status.DestroyTower();
    }

    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;
    public int GetNextLevelIndex() => SceneUtility.GetBuildIndexByScenePath(currentLevelName) + 1;
    public string GetNextLevelName() => "Level_" + GetNextLevelIndex();
    public bool HasNoMoreLevels() => GetNextLevelIndex() >= SceneManager.sceneCountInBuildSettings;
}