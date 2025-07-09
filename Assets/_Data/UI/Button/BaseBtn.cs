using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBtn : NhoxBehaviour
{
    [SerializeField] protected Button button;

    protected void OnEnable() => button.onClick.AddListener(OnClick);

    protected void OnDisable() => button.onClick.RemoveListener(OnClick);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadButton();
    }

    protected void LoadButton()
    {
        if (button != null) return;
        button = GetComponent<Button>();
        Debug.Log(transform.name + " :LoadButton", gameObject);
    }

    protected abstract void OnClick();
}