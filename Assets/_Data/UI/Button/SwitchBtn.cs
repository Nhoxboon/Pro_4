using UnityEngine;

public class SwitchBtn : BaseBtn
{
    protected enum SwitchMode { MainUI, PauseUI }
    
    [SerializeField] protected GameObject uiElement;
    [SerializeField] protected SwitchMode switchMode;

    protected override void OnClick() => SwitchUI();
    
    protected void SwitchUI()
    {
        if(ManagerCtrl.Instance.TileManager.IsGridMoving) return;
        switch (switchMode)
        {
            case SwitchMode.MainUI:
                ManagerCtrl.Instance.UI.SwitchToUI(uiElement);
                break;
            case SwitchMode.PauseUI:
                ManagerCtrl.Instance.UI.PauseUI.SwitchPauseUIElement(uiElement);
                break;
            default:
                break;
        }
    }
}