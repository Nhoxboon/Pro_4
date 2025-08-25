using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FXSpawner : Spawner
{
    private static FXSpawner instance;
    public static FXSpawner Instance => instance;
    
    private Coroutine fxCoroutine;

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

    public Transform SpawnParticle(string prefabName, Vector3 spawnPos, Quaternion rotation, float time = 1)
    {
        Transform fx = Spawn(prefabName, spawnPos, rotation);

        fx.gameObject.SetActive(true);
        if (!fx.TryGetComponent<FXParticleCache>(out var cache)) return fx;
        if (fxCoroutine != null)
            StopCoroutine(fxCoroutine);
        
        fxCoroutine = StartCoroutine(EnableFXCoroutine(cache, time));

        return fx;
    }

    private IEnumerator EnableFXCoroutine(FXParticleCache fx, float time)
    {
        fx.cachedParticle.Play();
        yield return new WaitForSeconds(time);
        fx.cachedParticle.Stop();
    }

    public void DespawnAllFX()
    {
        List<Transform> activeFX = holder.Cast<Transform>().Where(child => child.gameObject.activeSelf).ToList();

        foreach (Transform fx in activeFX)
            Despawn(fx.gameObject);

        if (fxCoroutine == null) return;
        StopCoroutine(fxCoroutine);
        fxCoroutine = null;
    }
}