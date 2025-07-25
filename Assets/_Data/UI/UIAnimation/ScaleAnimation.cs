using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleAnimation : NhoxBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] protected UIAnimator uiAnimator;

    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected float showcaseScale = 1.1f;
    [SerializeField] protected float scaleUpDuration = 0.25f;

    protected Coroutine scaleCoroutine;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIAnimator();
        LoadRectTransform();
    }

    protected void LoadUIAnimator()
    {
        if (uiAnimator != null) return;
        uiAnimator = GetComponentInParent<UIAnimator>();
        DebugTool.Log(transform.name + " :LoadUIAnimator", gameObject);
    }

    protected void LoadRectTransform()
    {
        if (rectTransform != null) return;
        rectTransform = GetComponent<RectTransform>();
        DebugTool.Log(transform.name + " :LoadRectTransform", gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);

        AudioManager.Instance?.PlaySFX(UI.Instance.onHoverSFX);
        scaleCoroutine = StartCoroutine(uiAnimator.ChangeScaleCoroutine(rectTransform, showcaseScale, scaleUpDuration));

        if (this.TryGetComponentInChildren<TextBlinkEffect>(out var textEff)) textEff.EnableBlink(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(uiAnimator.ChangeScaleCoroutine(rectTransform, 1f, scaleUpDuration));

        if (this.TryGetComponentInChildren<TextBlinkEffect>(out var textEff)) textEff?.EnableBlink(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(TileManager.Instance.IsGridMoving) return;
        
        AudioManager.Instance?.PlaySFX(UI.Instance.onClickSFX);
        rectTransform.localScale = Vector3.one;
    }
}