using UnityEngine;

public class ForceWaveBtn : BaseBtn
{
    protected override void OnClick() => EnemySpawnCoordinator.Instance.StartNewWave();
}