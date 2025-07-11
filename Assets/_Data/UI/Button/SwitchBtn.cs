using UnityEngine;

public class SwitchBtn : BaseBtn
{
    protected enum SwitchMode { MainUI, PauseUI }
    
    [SerializeField] protected GameObject uiElement;
    [SerializeField] protected SwitchMode switchMode;

    protected override void OnClick()
    {
        switch (switchMode)
        {
            case SwitchMode.MainUI:
                UI.Instance.SwitchToUI(uiElement);
                break;
            case SwitchMode.PauseUI:
                UI.Instance.PauseUI.SwitchPauseUIElement(uiElement);
                break;
            default:
                break;
        }
    }
}