using System.Collections;
using UnityEngine;

public abstract class TowerVisual : TowerComponent
{
    protected void SpawnVFX(string fxName, Vector3 position, Quaternion rotation)
    {
        var vfx = FXSpawner.Instance.Spawn(fxName, position, rotation);
        vfx.gameObject.SetActive(true);
    }
}