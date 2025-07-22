using UnityEngine;
using TMPro;

public class BuildButtonUI : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI towerNameText;

    [SerializeField] protected TextMeshProUGUI towerPriceText;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTowerNameText();
        LoadTowerPriceText();
    }

    protected void LoadTowerNameText()
    {
        if (towerNameText != null) return;
        towerNameText = transform.Find("TowerName").GetComponent<TextMeshProUGUI>();
        DebugTool.Log(transform.name + " :LoadTowerNameText", gameObject);
    }

    protected void LoadTowerPriceText()
    {
        if (towerPriceText != null) return;
        towerPriceText = transform.Find("TowerPrice").GetComponent<TextMeshProUGUI>();
        DebugTool.Log(transform.name + " :LoadTowerPriceText", gameObject);
    }

    public void SetInfo(string towerName, int towerPrice)
    {
        towerNameText.text = towerName;
        towerPriceText.text = towerPrice.ToString();
    }
}