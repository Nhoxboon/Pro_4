using System;
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

    protected void Update()
    {
        //For testing purposes
        if (Input.GetKeyDown(KeyCode.B)) ShowBtn();
    }

    public void ShowBtn()
    {
        isBuildMenuActive = !isBuildMenuActive;

        float yOffset = isBuildMenuActive ? yPosOffset : -yPosOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0f;

        uiAnimator.ChangePos(transform, new Vector3(0, yOffset), openAnimationDuration);

        Invoke(nameof(ToggleBtnMovement), methodDelay);
    }

    protected void ToggleBtnMovement()
    {
        foreach (var btn in buildBtns)
            btn.ToggleMovement(isBuildMenuActive);
    }
}