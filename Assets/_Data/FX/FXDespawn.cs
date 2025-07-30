
using UnityEngine;

public class FXDespawn : DespawnByTime
{
    public override void DespawnObject() => FXSpawner.Instance.Despawn(transform.parent.gameObject);
}
