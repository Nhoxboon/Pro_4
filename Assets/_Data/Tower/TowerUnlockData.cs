[System.Serializable]
public class TowerUnlockData
{
    public string towerName;
    public bool unlocked;

    public TowerUnlockData(string towerName, bool unlocked)
    {
        this.towerName = towerName;
        this.unlocked = unlocked;
    }
}