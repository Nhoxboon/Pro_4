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

    protected override void SetInstance() => _instance = this;

    protected void Update()
    {
        if (!gameBegun) return;
        UpdateWaveTimer();
    }

    [ContextMenu("Activate Wave Manager")]
    public void ActivateWaveManager()
    {
        gameBegun = true;
        EnableWaveTimer(true);
    }

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

    public void AdvanceToNextWave() => currentWaveIndex++;

    public bool HasNoMoreWaves() => currentWaveIndex >= levelWave.Length;
}