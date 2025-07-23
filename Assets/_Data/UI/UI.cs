using UnityEngine;

public class UI : NhoxBehaviour
{
    private static UI instance;
    public static UI Instance => instance;

    [SerializeField] protected GameObject[] uiElements;
    [SerializeField] protected UIAnimator uiAnimator;
    public UIAnimator UiAnimator => uiAnimator;
    [Space] [SerializeField] protected MenuUI menuUI;
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
            // DebugTool.LogError("Only one instance of UI allow to exist");
            return;
        }

        instance = this;

        SwitchToUI(settingsUI.gameObject);
        SwitchToUI(inGameUI.gameObject);
        SwitchToUI(menuUI.gameObject);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIElements();
        LoadUIAnimator();
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

        DebugTool.Log(transform.name + " :LoadUIElements", gameObject);
    }

    protected void LoadUIAnimator()
    {
        if (uiAnimator != null) return;
        uiAnimator = GetComponent<UIAnimator>();
        DebugTool.Log(transform.name + " :LoadUIAnimator", gameObject);
    }

    protected void LoadMenuUI()
    {
        if (menuUI != null) return;
        menuUI = GetComponentInChildren<MenuUI>(true);
        DebugTool.Log(transform.name + " :LoadMenuUI", gameObject);
    }

    protected void LoadSettingsUI()
    {
        if (settingsUI != null) return;
        // Ensure that SettingsUI is below PauseUI in the hierarchy
        settingsUI = GetComponentInChildren<SettingsUI>(true);
        DebugTool.Log(transform.name + " :LoadSettingsUI", gameObject);
    }

    protected void LoadInGameUI()
    {
        if (inGameUI != null) return;
        inGameUI = GetComponentInChildren<InGameUI>(true);
        DebugTool.Log(transform.name + " :LoadInGameUI", gameObject);
    }

    protected void LoadPauseUI()
    {
        if (pauseUI != null) return;
        pauseUI = GetComponentInChildren<PauseUI>(true);
        DebugTool.Log(transform.name + " :LoadPauseUI", gameObject);
    }

    protected void LoadFadeImage()
    {
        if (fadeImage != null) return;
        fadeImage = GetComponentInChildren<FadeImage>(true);
        DebugTool.Log(transform.name + " :LoadFadeImage", gameObject);
    }

    public void SwitchToUI(GameObject uiEnable)
    {
        for (int i = 0; i < uiElements.Length; i++) uiElements[i].SetActive(false);

        if (uiEnable is not null) uiEnable.SetActive(true);
    }
    
    public void EnableMenuUI(bool enable) => SwitchToUI(enable ? menuUI.gameObject : null);
    
    public void EnableSettingsUI(bool enable) => SwitchToUI(enable ? settingsUI.gameObject : null);
    
    public void EnableInGameUI(bool enable) => SwitchToUI(enable ? inGameUI.gameObject : null);
    
    public void EnablePauseUI(bool enable) => SwitchToUI(enable ? pauseUI.gameObject : null);
}