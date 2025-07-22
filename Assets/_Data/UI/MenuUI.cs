
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : NhoxBehaviour
{
    [SerializeField] protected Button[] buttons;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadButtons();
    }
    
    protected void LoadButtons()
    {
        if (buttons is { Length: > 0 }) return;
        buttons = GetComponentsInChildren<Button>(true);
        DebugTool.Log(transform.name + " :LoadButtons", gameObject);
    }
}
