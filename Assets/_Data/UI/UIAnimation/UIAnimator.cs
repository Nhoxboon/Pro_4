using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : NhoxBehaviour
{
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

    public void ChangeColor(Image image, float targetAlpha, float duration)
    {
        StartCoroutine(ChangeColorCoroutine(image, targetAlpha, duration));
    }

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