using System;
using UnityEngine;

public class WaveTimingManager : WaveSystemManager
{
    private static WaveTimingManager _instance;
    public static WaveTimingManager Instance => _instance;

    public Action OnWaveTimerUpdated;

    [Header("Wave Timing")] [SerializeField]
    protected float timeBetweenWaves = 10f;

    [SerializeField] protected float waveTimer;
    public float WaveTimer => waveTimer;
    protected bool waveTimerEnabled;
    public bool WaveTimerEnabled => waveTimerEnabled;

    [SerializeField] protected int currentWaveIndex;
    public int CurrentWaveIndex => currentWaveIndex;
    [SerializeField] protected WaveDetails[] levelWave;
    public WaveDetails[] LevelWave => levelWave;

    protected bool gameBegun;
    public bool GameBegun => gameBegun;

    protected override void SetInstance() => _instance = this;

    protected void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.T)) ActivateWaveManager();
#endif
        if (!gameBegun) return;
        UpdateWaveTimer();
    }

    [ContextMenu("Activate Wave Manager")]
    public void ActivateWaveManager()
    {
        gameBegun = true;
        EnableWaveTimer(true);
    }

    public void DeactivateWaveManager() => gameBegun = false;

    protected void UpdateWaveTimer()
    {
        if (!waveTimerEnabled) return;
        waveTimer -= Time.deltaTime;
        OnWaveTimerUpdated?.Invoke();
        if (waveTimer <= 0f) EnemySpawnCoordinator.Instance.StartNewWave();
    }

    public void EnableWaveTimer(bool enable)
    {
        if (waveTimerEnabled == enable) return;

        waveTimerEnabled = enable;
        waveTimer = timeBetweenWaves;
        OnWaveTimerUpdated?.Invoke();
    }

    public void ResetWaveManager()
    {
        gameBegun = false;
        EnableWaveTimer(false);
        currentWaveIndex = 0;
        waveTimer = timeBetweenWaves;
    }

    public void AdvanceToNextWave() => currentWaveIndex++;

    public bool HasNoMoreWaves() => currentWaveIndex >= levelWave.Length;
}