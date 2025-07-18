using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : NhoxBehaviour
{
    [SerializeField] protected float adjustmentSpeed = 10f;

    [SerializeField] protected float showcaseY = 215f;
    [SerializeField] protected float defaultY = 175f;

    protected float targetY;
    protected bool canMove;

    protected void Update() => Hover();

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

    public void ShowcaseBtn(bool showcase)
    {
        SetTargetY(showcase ? showcaseY : defaultY);
    }
}