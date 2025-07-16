using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TowerUnlockConfig", menuName = "ScriptableObject/Tower Data")]
public class TowerUnlockConfigSO : ScriptableObject
{
    public List<TowerUnlockData> towerUnlockList;

    [ContextMenu("Initialize Tower Data")]
    public void InitializeTowerData()
    {
        towerUnlockList = new List<TowerUnlockData>
        {
            new TowerUnlockData("Crossbow", false),
            new TowerUnlockData("Canon", false),
            new TowerUnlockData("Rapid Fire Gun", false),
            new TowerUnlockData("Hammer", false),
            new TowerUnlockData("Spider Nest", false),
            new TowerUnlockData("AA Harpoon", false),
            new TowerUnlockData("Only Fan", false)
        };
    }
}