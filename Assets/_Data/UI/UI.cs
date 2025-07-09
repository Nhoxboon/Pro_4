using UnityEngine;

public class UI : NhoxBehaviour
{
    private static UI instance;
    public static UI Instance => instance;

    [SerializeField] protected GameObject[] uiElements;
    public GameObject[] UIElements => uiElements;
    
    [SerializeField] protected MenuUI menuUI;
    [SerializeField] protected SettingsUI settingsUI;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one instance of UI allow to exist");
            return;
        }

        instance = this;
        
        SwitchToUI(settingsUI.gameObject);
        SwitchToUI(menuUI.gameObject);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIElements();
        LoadMenuUI();
        LoadSettingsUI();
    }

    protected void LoadUIElements()
    {
        if (uiElements is { Length: > 0 }) return;
        int childCount = transform.childCount;
        uiElements = new GameObject[childCount];

        for (int i = 0; i < childCount; i++) uiElements[i] = transform.GetChild(i).gameObject;

        Debug.Log(transform.name + " :LoadUIElements", gameObject);
    }
    
    protected void LoadMenuUI()
    {
        if (menuUI != null) return;
        menuUI = GetComponentInChildren<MenuUI>(true);
        Debug.Log(transform.name + " :LoadMenuUI", gameObject);
    }
    
    protected void LoadSettingsUI()
    {
        if (settingsUI != null) return;
        settingsUI = GetComponentInChildren<SettingsUI>(true);
        Debug.Log(transform.name + " :LoadSettingsUI", gameObject);
    }

    public void SwitchToUI(GameObject uiEnable)
    {
        foreach (var ui in uiElements) ui.SetActive(false);

        uiEnable.SetActive(true);
    }
}