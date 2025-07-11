using UnityEngine;

public class UI : NhoxBehaviour
{
    private static UI instance;
    public static UI Instance => instance;

    [SerializeField] protected GameObject[] uiElements;
    public GameObject[] UIElements => uiElements;

    [SerializeField] protected MenuUI menuUI;
    [SerializeField] protected SettingsUI settingsUI;
    [SerializeField] protected InGameUI inGameUI;
    public InGameUI InGameUI => inGameUI;
    [SerializeField] protected PauseUI pauseUI;
    public PauseUI PauseUI => pauseUI;
    [SerializeField] protected FadeImage fadeImage;

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
        SwitchToUI(inGameUI.gameObject);
        // SwitchToUI(menuUI.gameObject);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIElements();
        LoadMenuUI();
        LoadSettingsUI();
        LoadInGameUI();
        LoadPauseUI();
        LoadFadeImage();
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
        // Ensure that SettingsUI is below PauseUI in the hierarchy
        settingsUI = GetComponentInChildren<SettingsUI>(true);
        Debug.Log(transform.name + " :LoadSettingsUI", gameObject);
    }

    protected void LoadInGameUI()
    {
        if (inGameUI != null) return;
        inGameUI = GetComponentInChildren<InGameUI>(true);
        Debug.Log(transform.name + " :LoadInGameUI", gameObject);
    }

    protected void LoadPauseUI()
    {
        if (pauseUI != null) return;
        pauseUI = GetComponentInChildren<PauseUI>(true);
        Debug.Log(transform.name + " :LoadPauseUI", gameObject);
    }

    protected void LoadFadeImage()
    {
        if (fadeImage != null) return;
        fadeImage = GetComponentInChildren<FadeImage>(true);
        Debug.Log(transform.name + " :LoadFadeImage", gameObject);
    }

    public void SwitchToUI(GameObject uiEnable)
    {
        foreach (var ui in uiElements) ui.SetActive(false);

        uiEnable.SetActive(true);
    }
}