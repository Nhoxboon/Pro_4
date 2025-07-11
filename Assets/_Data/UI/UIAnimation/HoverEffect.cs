﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : NhoxBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected float adjustmentSpeed = 10f;

    [SerializeField] protected float showcaseY;
    [SerializeField] protected float defaultY;

    protected float targetY;
    protected bool canMove;

    protected void Update()
    {
        // if (!canMove) return;
        Hover();
    }

    protected void Hover()
    {
        if (Mathf.Abs(targetY - transform.position.y) > 0.01f && canMove)
        {
            float newPosY = Mathf.Lerp(transform.position.y, targetY, adjustmentSpeed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
        }
    }

    public void ToggleMovement(bool btnMenuActive)
    {
        canMove = btnMenuActive;
        SetTargetY(defaultY);

        if (!btnMenuActive) SetPositionToDefault();
    }

    protected void SetPositionToDefault() =>
        transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);

    protected void SetTargetY(float newTargetY) => targetY = newTargetY;

    public void OnPointerEnter(PointerEventData eventData) => SetTargetY(showcaseY);

    public void OnPointerExit(PointerEventData eventData) => SetTargetY(defaultY);
}