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
            Debug.LogError("Only one TowerSpawner allowed to exist");
            return;
        }

        instance = this;
    }

    public float GetAttackRange(string towerName)
    {
        Transform prefab = GetPrefabByName(towerName);
        return prefab?.GetComponent<Tower>()?.AttackRange ?? 0f;
    }
}