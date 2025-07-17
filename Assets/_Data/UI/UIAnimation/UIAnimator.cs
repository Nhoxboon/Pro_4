using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : NhoxBehaviour
{
    [Header("UI Feedback")] [SerializeField]
    protected float shakeMagnitude = 5f;
    [SerializeField] protected float shakeDuration = 0.25f;
    [SerializeField] protected float shakeRotationMagnitude = 3f;
    [Space]
    [SerializeField] protected float defaultUIScale = 1.75f;
    [SerializeField] protected bool scaleAvailable;

    public void Shake(Transform transform)
    {
        RectTransform rectTransform = transform as RectTransform;
        StartCoroutine(ShakeCoroutine(rectTransform));
    }

    private IEnumerator ShakeCoroutine(RectTransform rectTransform)
    {
        float time = 0f;
        Vector3 originalPosition = rectTransform.anchoredPosition;
        float currentUIScale = rectTransform.localScale.x;

        if (scaleAvailable)
            StartCoroutine(ChangeScaleCoroutine(rectTransform, currentUIScale * 1.1f, shakeDuration / 2));
        while (time < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float rotationOffset = Random.Range(-shakeRotationMagnitude, shakeRotationMagnitude);

            rectTransform.anchoredPosition = originalPosition + new Vector3(xOffset, yOffset);
            rectTransform.localRotation = Quaternion.Euler(0, 0, rotationOffset);

            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
        if (scaleAvailable)
            StartCoroutine(ChangeScaleCoroutine(rectTransform, defaultUIScale, shakeDuration / 2));
    }

    public void ChangePos(Transform transform, Vector3 offset, float duration = 0.1f)
    {
        RectTransform rectTransform = transform as RectTransform;
        StartCoroutine(ChangePosCoroutine(rectTransform, offset, duration));
    }

    private IEnumerator ChangePosCoroutine(RectTransform rectTransform, Vector3 offset, float duration)
    {
        float time = 0;

        Vector3 initialPosition = rectTransform.anchoredPosition;
        Vector3 targetPosition = initialPosition + offset;

        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;

            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
    }

    public void ChangeScale(Transform transform, float targetScale, float duration = 0.25f)
    {
        RectTransform rectTransform = transform as RectTransform;
        StartCoroutine(ChangeScaleCoroutine(rectTransform, targetScale, duration));
    }

    public IEnumerator ChangeScaleCoroutine(RectTransform rectTransform, float newScale, float duration = 0.25f)
    {
        float time = 0f;
        Vector3 initialScale = rectTransform.localScale;
        Vector3 targetScale = new Vector3(newScale, newScale, newScale);

        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime;

            yield return null;
        }
        rectTransform.localScale = targetScale;
    }

    public void ChangeColor(Image image, float targetAlpha, float duration) =>
        StartCoroutine(ChangeColorCoroutine(image, targetAlpha, duration));


    private IEnumerator ChangeColorCoroutine(Image image, float targetAlpha, float duration)
    {
        float time = 0f;
        Color currentColor = image.color;
        float startAlpha = currentColor.a;

        while (time < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            time += Time.deltaTime;

            yield return null;
        }
        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}