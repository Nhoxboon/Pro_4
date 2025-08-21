public class FadeImage : BaseImage
{
    public void ActivateFadeEffect(bool fadeIn)
    {
        if(!gameObject.activeSelf) return;
        if (fadeIn)
            uiAnimator.ChangeColor(image, 0f, 2f);
        else
            uiAnimator.ChangeColor(image, 1f, 2f);
    }
}