public class FadeImage : BaseImage
{
    protected override void Awake()
    {
        base.Awake();
        ActivateFadeEffect(true);
    }

    public void ActivateFadeEffect(bool fadeIn)
    {
        if (fadeIn)
            uiAnimator.ChangeColor(image, 0f, 2f);
        else
            uiAnimator.ChangeColor(image, 1f, 2f);
    }
}