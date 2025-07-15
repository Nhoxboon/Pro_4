using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : NhoxBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    protected Vector3 defaultPosition;
    protected bool tileCanMove = true;

    protected Coroutine currentMoveUpCoroutine;

    protected override void Awake()
    {
        base.Awake();
        defaultPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InputManager.Instance.IsRightMouseHeld || InputManager.Instance.IsMiddleMouseHeld ||
            InputManager.Instance.IsMiddleMouseDown) return;

        if (!tileCanMove) return;
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!tileCanMove) return;

        if (currentMoveUpCoroutine != null) Invoke(nameof(MoveTileDown), TileManager.Instance.YMovementDuration);
        else
            MoveTileDown();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;

        if(BuildManager.Instance.SelectedBuildSlot == this) return;

        BuildManager.Instance.EnableBuildMenu();
        BuildManager.Instance.SelectBuildSlot(this);
        MoveTileUp();
        tileCanMove = false;
    }

    protected void MoveTileUp()
    {
        var targetPosition = transform.position + new Vector3(0, TileManager.Instance.BuildSlotYOffset, 0);
        currentMoveUpCoroutine = StartCoroutine(TileManager.Instance.TileMoveCoroutine(transform, targetPosition));
    }

    protected void MoveTileDown() => TileManager.Instance.MoveTile(transform, defaultPosition);

    public void UnSelectTile()
    {
        MoveTileDown();
        tileCanMove = true;
    }
}