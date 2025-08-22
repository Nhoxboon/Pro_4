using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NhoxBehaviour
{
    protected GridBuilder currentActiveGrid;
    [SerializeField] protected string currentLevelName;
    [SerializeField] protected CameraEffects cameraEffects;

    [Header("Color change Details")] [SerializeField]
    protected MeshRenderer groundMesh;

    protected Color defaultColor;

    protected override void Awake()
    {
        base.Awake();
        defaultColor = groundMesh.material.color;
        groundMesh.material = new Material(groundMesh.material);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraEffects();
        LoadGround();
    }

    protected void LoadCameraEffects()
    {
        if (cameraEffects != null) return;
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        DebugTool.Log(transform.name + " :LoadCameraEffects", gameObject);
    }

    protected void LoadGround()
    {
        if (groundMesh != null) return;
        groundMesh = GameObject.Find("Ground").GetComponent<MeshRenderer>();
        DebugTool.Log(transform.name + " :LoadGround", gameObject);
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

        UpdateGroundColor(defaultColor);
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
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].Core.Death.DestroyEnemy();
    }

    protected void EliminateAllTowers()
    {
        TowerCtrl[] towers = FindObjectsByType<TowerCtrl>(FindObjectsSortMode.None);
        for (int i = 0; i < towers.Length; i++)
            towers[i].Status.DestroyTower();
    }

    public void UpdateCurrentGrid(GridBuilder newGrid) => currentActiveGrid = newGrid;
    public int GetNextLevelIndex() => SceneUtility.GetBuildIndexByScenePath(currentLevelName) + 1;
    public string GetNextLevelName() => "Level_" + GetNextLevelIndex();
    public bool HasNoMoreLevels() => GetNextLevelIndex() >= SceneManager.sceneCountInBuildSettings;

    public void UpdateGroundColor(Color targetColor) => StartCoroutine(UpdateGroundColorCoroutine(targetColor, 1.5f));

    protected IEnumerator UpdateGroundColorCoroutine(Color targetColor, float duration)
    {
        float time = 0f;
        Color startColor = groundMesh.material.color;

        while (time < duration)
        {
            Color currentColor = Color.Lerp(startColor, targetColor, time / duration);
            groundMesh.material.color = currentColor;
            time += Time.deltaTime;
            yield return null;
        }

        groundMesh.material.color = targetColor;
    }
}