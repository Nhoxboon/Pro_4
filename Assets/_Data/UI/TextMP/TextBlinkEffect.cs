using System;
using TMPro;
using UnityEngine;

public class TextBlinkEffect : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI textMeshPro;

    [SerializeField] protected float changeValueSpeed = 3.5f;
    protected float targetAlpha;

    protected bool canBlink;

    protected void Update() => UpdateBlinking();

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

    public void EnableBlink(bool enable)
    {
        canBlink = enable;
        if (!canBlink) ChangeColorAlpha(1);
    }

    protected void ChangeTargetAlpha() => targetAlpha = Mathf.Approximately(targetAlpha, 1) ? 0 : 1;

    protected void ChangeColorAlpha(float newAlpha)
    {
        Color color = textMeshPro.color;
        textMeshPro.color = new Color(color.r, color.g, color.b, newAlpha);
    }

    protected void UpdateBlinking()
    {
        if (!canBlink) return;

        if (Mathf.Abs(textMeshPro.color.a - targetAlpha) > 0.01f)
        {
            var newAlpha = Mathf.Lerp(textMeshPro.color.a, targetAlpha, changeValueSpeed * Time.deltaTime);
            ChangeColorAlpha(newAlpha);
        }
        else
            ChangeTargetAlpha();
    }
}