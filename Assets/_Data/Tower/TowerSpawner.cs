using UnityEngine;

public class TowerSpawner : Spawner
{
    private static TowerSpawner instance;
    public static TowerSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one instance of TowerSpawner allowed to exist");
            return;
        }

        instance = this;
    }
}
