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

    [SerializeField] protected int nextWaveIndex;
    [SerializeField] protected WaveDetails[] levelWave;
    public WaveDetails[] LevelWave => levelWave;
    public int NextWaveIndex => nextWaveIndex;

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

    public void AdvanceToNextWave() => nextWaveIndex++;

    public bool HasNoMoreWaves() => nextWaveIndex >= levelWave.Length;
}