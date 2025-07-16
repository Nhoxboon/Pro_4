using System.Collections;
using UnityEngine;

public class BuildBtnsUI : NhoxBehaviour
{
    [SerializeField] protected UIAnimator uiAnimator;

    protected bool isBuildMenuActive;
    [SerializeField] protected float yPosOffset = 270f;
    [SerializeField] protected float openAnimationDuration = 0.1f;

    [SerializeField] protected HoverEffect[] buildBtnEffects;
    [SerializeField] protected BuildBtn[] buildBtns;
    public BuildBtn[] BuildBtns => buildBtns;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIAnimator();
        LoadBuildBtnsEffect();
        LoadBuildBtns();
    }

    protected void LoadUIAnimator()
    {
        if (uiAnimator != null) return;
        uiAnimator = GetComponentInParent<UIAnimator>();
        Debug.Log(transform.name + " :LoadUIAnimator", gameObject);
    }

    protected void LoadBuildBtnsEffect()
    {
        if (buildBtnEffects != null && buildBtnEffects.Length > 0) return;
        buildBtnEffects = GetComponentsInChildren<HoverEffect>();
        Debug.Log(transform.name + " :LoadBuildBtnsEffect", gameObject);
    }

    protected void LoadBuildBtns()
    {
        if (buildBtns != null && buildBtns.Length > 0) return;
        buildBtns = GetComponentsInChildren<BuildBtn>();
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
        for (int i = 0; i < buildBtnEffects.Length; i++)
            buildBtnEffects[i].ToggleMovement(isBuildMenuActive);
    }

    private IEnumerator DelayedToggleBtnMovement(float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        ToggleBtnMovement();
    }
}