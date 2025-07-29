using UnityEngine;

public class HeavyEnemyCore : Core
{
    protected override void Awake()
    {
        base.Awake();
        shieldObject.gameObject.SetActive(true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadShield();
    }

    protected void LoadShield()
    {
        if (shieldObject != null) return;
        shieldObject = GetComponentInChildren<ShieldForEnemy>(true);
        DebugTool.Log(transform.name + " :LoadShield", gameObject);
    }
}