using System;
using System.Collections;
using UnityEngine;

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

    protected int enemyKilled;
    public int EnemyKilled => enemyKilled;

    [SerializeField] protected CameraEffects cameraEffects;
    [SerializeField] protected WaveTimingManager currentWaveManager;
    public Action OnHPChanged;
    public Action OnCurrencyChanged;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            DebugTool.LogError("Only one GameManager allowed to exist");
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

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCameraEffects();
    }

    protected void LoadCameraEffects()
    {
        if (cameraEffects != null) return;
        cameraEffects = FindFirstObjectByType<CameraEffects>();
        DebugTool.Log(transform.name + " LoadCameraEffects", gameObject);
    }

    public void LevelCompleted() => StartCoroutine(LevelCompletedCoroutine());

    public IEnumerator LevelCompletedCoroutine()
    {
        cameraEffects.FocusOnCastle();
        yield return cameraEffects.CameraCoroutine;

        if (LevelManager.Instance.HasNoMoreLevels())
            UI.Instance.InGameUI.EnableVictoryUI(true);
        else
        {
            PlayerPrefs.SetInt(LevelManager.Instance.GetNextLevelName() + "unlocked", 1);
            UI.Instance.InGameUI.EnableLevelCompletedUI(true);
        }
    }

    public IEnumerator LevelFailedCoroutine()
    {
        isInGame = false;
        currentWaveManager.DeactivateWaveManager();
        cameraEffects.FocusOnCastle();

        yield return cameraEffects.CameraCoroutine;
        UI.Instance.InGameUI.EnableGameOverUI(true);
    }

    public void UpdateGameManager(int levelCurrency, WaveTimingManager newWaveManager)
    {
        isInGame = true;
        enemyKilled = 0;
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

        if (currentHP <= 0 || !isInGame) StartCoroutine(LevelFailedCoroutine());
    }

    public void UpdateCurrency(int amount)
    {
        enemyKilled++;
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