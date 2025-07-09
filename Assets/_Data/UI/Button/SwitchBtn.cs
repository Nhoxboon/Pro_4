using UnityEngine;

public class SwitchBtn : BaseBtn
{
    [SerializeField] protected GameObject uiElement;

    protected override void OnClick() => UI.Instance.SwitchToUI(uiElement);
}