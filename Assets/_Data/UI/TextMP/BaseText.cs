using System;
using TMPro;
using UnityEngine;

public abstract class BaseText : NhoxBehaviour
{
    [SerializeField] protected UIAnimator uiAnimator;
    [SerializeField] protected TextMeshProUGUI textMeshPro;
    

    protected abstract void OnDestroy();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIAnimator();
        LoadTextMeshPro();
    }

    protected void LoadUIAnimator()
    {
        if(uiAnimator != null) return;
        uiAnimator = GetComponentInParent<UIAnimator>();
        DebugTool.Log(transform.name + " :LoadUIAnimator", gameObject);
    }

    protected void LoadTextMeshPro()
    {
        if (textMeshPro != null) return;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        DebugTool.Log(transform.name + " :LoadTextMeshPro", gameObject);
    }
}