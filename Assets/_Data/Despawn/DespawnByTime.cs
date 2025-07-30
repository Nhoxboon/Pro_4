using UnityEngine;

public abstract class DespawnByTime : Despawn
{
    [SerializeField] protected float existTime = 2f; 
    protected float startTime;

    protected virtual void OnEnable()
    {
        startTime = Time.time; 
    }

    protected override bool CanDespawn()
    {
        return (Time.time - startTime >= existTime);
    }
}