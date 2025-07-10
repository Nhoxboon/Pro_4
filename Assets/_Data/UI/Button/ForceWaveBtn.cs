using UnityEngine;

public class ForceWaveBtn : BaseBtn
{
    protected override void OnClick() => WaveManager.Instance.ForceNextWave();
}