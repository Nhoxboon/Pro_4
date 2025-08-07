using UnityEngine;

public class ProjectileDespawn : DespawnByTime
{
    public override void DespawnObject() => ProjectileSpawner.Instance.Despawn(transform.parent.gameObject);

}
