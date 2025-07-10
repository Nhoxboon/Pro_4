using System;
using TMPro;
using UnityEngine;

public abstract class BaseText : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI textMeshPro;

    protected abstract void OnDestroy();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTextMeshPro();
    }

    protected void LoadTextMeshPro()
    {
        if (textMeshPro != null) return;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + " :LoadTextMeshPro", gameObject);
    }
}