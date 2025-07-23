using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NhoxBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] protected int currency = 500;
    public int Currency => currency;
    [SerializeField] protected int maxHP = 100;
    public int MaxHP => maxHP;
    [SerializeField] protected int currentHP;
    public int CurrentHP => currentHP;

    protected bool isInGame;
    public bool IsInGame => isInGame;

    public Action OnHPChanged;
    public Action OnCurrencyChanged;
    [SerializeField] protected WaveTimingManager currentWaveManager;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one GameManager allowed to exist");
            return;
        }

        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        currentHP = maxHP;
        OnHPChanged?.Invoke();
        OnCurrencyChanged?.Invoke();
    }

    public void LevelCompleted()
    {
        string currentLevelName = LevelManager.Instance.CurrentLevelName;
        int nextLevelIndex = SceneUtility.GetBuildIndexByScenePath(currentLevelName) + 1;
        if (nextLevelIndex >= SceneManager.sceneCountInBuildSettings)
            UI.Instance.InGameUI.EnableVictoryUI(true);
        else
            LevelManager.Instance.LoadLevel("Level_" + nextLevelIndex);
    }

    public void LevelFailed()
    {
        if (!isInGame) return;
        isInGame = false;
        currentWaveManager.DeactivateWaveManager();
        UI.Instance.InGameUI.EnableGameOverUI(true);
    }

    public void UpdateGameManager(int levelCurrency, WaveTimingManager newWaveManager)
    {
        isInGame = true;
        currentWaveManager = newWaveManager;
        currency = levelCurrency;
        currentHP = maxHP;
        OnCurrencyChanged?.Invoke();
        OnHPChanged?.Invoke();
    }

    public void UpdateHP(int amount)
    {
        currentHP += amount;
        OnHPChanged?.Invoke();
        UI.Instance.InGameUI.ShakeHPUI();
        if (currentHP <= 0) LevelFailed();
    }

    public void UpdateCurrency(int amount)
    {
        currency += amount;
        OnCurrencyChanged?.Invoke();
    }

    public bool HasEnoughCurrency(int amount)
    {
        if (amount > currency) return false;
        currency -= amount;
        OnCurrencyChanged?.Invoke();
        return true;
    }
}