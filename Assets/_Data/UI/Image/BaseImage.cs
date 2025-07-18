﻿using UnityEngine;
using UnityEngine.UI;

public abstract class BaseImage : NhoxBehaviour
{
    [SerializeField] protected Image image;
    [SerializeField] protected UIAnimator uiAnimator;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadImage();
        LoadUIAnimator();
    }

    protected void LoadImage()
    {
        if (image != null) return;
        image = GetComponent<Image>();
        Debug.Log(transform.name + " :LoadImage", gameObject);
    }

    protected void LoadUIAnimator()
    {
        if (uiAnimator != null) return;
        uiAnimator = GetComponentInParent<UIAnimator>();
        Debug.Log(transform.name + " :LoadUIAnimator", gameObject);
    }
}