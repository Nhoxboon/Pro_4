using System.Collections;
using UnityEngine;

public class FXSpawner : Spawner
{
    private static FXSpawner instance;
    public static FXSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            // DebugTool.LogError("Only one instance of FXSpawner allowed to exist");
            return;
        }

        instance = this;
    }

    public void DespawnByTime(Transform fx, float time) => StartCoroutine(DespawnAfterTime(fx, time));

    private IEnumerator DespawnAfterTime(Transform fx, float time)
    {
        yield return new WaitForSeconds(time);
        Despawn(fx.gameObject);
    }
}