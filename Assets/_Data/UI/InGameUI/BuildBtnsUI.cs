using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBtnsUI : NhoxBehaviour
{
    protected bool isBuildMenuActive;
    [SerializeField] protected float yPosOffset = 270f;
    [SerializeField] protected float openAnimationDuration = 0.1f;

    [SerializeField] protected HoverEffect[] buildBtnEffects;
    [SerializeField] protected BuildBtn[] buildBtns;
    public BuildBtn[] BuildBtns => buildBtns;
    [SerializeField] protected List<BuildBtn> unlockedBtn;
    protected BuildBtn lastSelectedBtn;
    public BuildBtn LastSelectedBtn => lastSelectedBtn;
    
    protected Transform previewTower;

    protected void Update()
    {
        HandleButtonHotKeys();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBuildBtnsEffect();
        LoadBuildBtns();
    }

    protected void LoadBuildBtnsEffect()
    {
        if (buildBtnEffects != null && buildBtnEffects.Length > 0) return;
        buildBtnEffects = GetComponentsInChildren<HoverEffect>();
        DebugTool.Log(transform.name + " :LoadBuildBtnsEffect", gameObject);
    }

    protected void LoadBuildBtns()
    {
        if (buildBtns != null && buildBtns.Length > 0) return;
        buildBtns = GetComponentsInChildren<BuildBtn>();
        DebugTool.Log(transform.name + " :LoadBuildBtns", gameObject);
    }

    public void SetLastSelectedBtn(BuildBtn newLastSelectedBtn, Transform newPreview)
    {
        lastSelectedBtn = newLastSelectedBtn;
        previewTower = newPreview;
    }

    public void SelectNewBtn(int btnIndex)
    {
        if (btnIndex >= unlockedBtn.Count) return;
        foreach (var btn in unlockedBtn)
            btn.SelectBtn(false);

        BuildBtn selectedBtn = unlockedBtn[btnIndex];
        selectedBtn.SelectBtn(true);
    }

    protected void HandleButtonHotKeys()
    {
        if (!isBuildMenuActive) return;
        for (int i = 0; i < unlockedBtn.Count; i++)
        {
            if (!InputManager.Instance.IsNumberKeyDown[1 + i]) continue;
            SelectNewBtn(i);
            break;
        }

        if (lastSelectedBtn is null) return;
        if (InputManager.Instance.IsSpaceDown)
        {
            lastSelectedBtn.ConfirmBuildTower();
            previewTower = null;
        }
            
        if(InputManager.Instance.IsRotateTowerLeft)
            RotateTarget(previewTower, -90f);
            
        if(InputManager.Instance.IsRotateTowerRight)
            RotateTarget(previewTower, 90f);
    }

    protected void RotateTarget(Transform target, float angle)
    {
        if (target is null) return;
        target.Rotate(0, angle, 0);
        target.TryGetComponent(out TowerPreview tower);
        tower.ForwardAttackDisplay?.UpdateLines();
    }

    public void ShowBtn(bool showBtns)
    {
        isBuildMenuActive = showBtns;

        float yOffset = isBuildMenuActive ? yPosOffset : -yPosOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0f;

        ManagerCtrl.Instance.UI.UiAnimator.ChangePos(transform, new Vector3(0, yOffset), openAnimationDuration);

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

    public void UpdateUnlockBtn()
    {
        foreach (var btn in buildBtns)
            if (btn.BtnUnlocked)
                unlockedBtn.Add(btn);
    }
}