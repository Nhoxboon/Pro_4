using System;
using System.Collections;
using UnityEngine;

public class BuildsBtnUI : NhoxBehaviour
{
    [SerializeField] protected UIAnimator uiAnimator;

    protected bool isBuildMenuActive;
    [SerializeField] protected float yPosOffset = 270f;
    [SerializeField] protected float openAnimationDuration = 0.1f;

    [SerializeField] protected HoverEffect[] buildBtns;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIAnimator();
        LoadBuildBtns();
    }

    protected void LoadUIAnimator()
    {
        if (uiAnimator != null) return;
        uiAnimator = GetComponentInParent<UIAnimator>();
        Debug.Log(transform.name + " :LoadUIAnimator", gameObject);
    }

    protected void LoadBuildBtns()
    {
        if (buildBtns != null && buildBtns.Length > 0) return;
        buildBtns = GetComponentsInChildren<HoverEffect>();
        Debug.Log(transform.name + " :LoadBuildBtns", gameObject);
    }

    public void ShowBtn(bool showBtns)
    {
        isBuildMenuActive = showBtns;

        float yOffset = isBuildMenuActive ? yPosOffset : -yPosOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0f;

        uiAnimator.ChangePos(transform, new Vector3(0, yOffset), openAnimationDuration);

        StopCoroutine(nameof(DelayedToggleBtnMovement));
        StartCoroutine(DelayedToggleBtnMovement(methodDelay));
    }

    protected void ToggleBtnMovement()
    {
        for (int i = 0; i < buildBtns.Length; i++)
            buildBtns[i].ToggleMovement(isBuildMenuActive);
    }

    private IEnumerator DelayedToggleBtnMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleBtnMovement();
    }
}