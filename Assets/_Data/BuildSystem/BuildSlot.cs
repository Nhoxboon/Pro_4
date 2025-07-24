using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildSlot : NhoxBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    protected Vector3 defaultPosition;
    protected bool tileCanMove = true;
    protected bool buildSlotAvailable = true;

    protected Coroutine currentMoveUpCoroutine;
    protected Coroutine moveToDefaultCoroutine;

    protected override void Awake()
    {
        base.Awake();
        defaultPosition = transform.position;
    }

    protected override void Start()
    {
        base.Start();
        if (!buildSlotAvailable)
            transform.position += new Vector3(0, 0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!buildSlotAvailable || !tileCanMove || TileManager.Instance.IsGridMoving) return;
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!tileCanMove || !buildSlotAvailable || TileManager.Instance.IsGridMoving) return;

        if (currentMoveUpCoroutine != null) Invoke(nameof(MoveTileDown), TileManager.Instance.DefaultMoveDuration);
        else
            MoveTileDown();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !buildSlotAvailable ||
            BuildManager.Instance.SelectedBuildSlot == this || TileManager.Instance.IsGridMoving ||
            !GameManager.Instance.IsInGame) return;

        BuildManager.Instance.EnableBuildMenu();
        BuildManager.Instance.SelectBuildSlot(this);
        MoveTileUp();
        tileCanMove = false;
        UI.Instance.InGameUI.BuildsBtnsUI.LastSelectedBtn?.SelectBtn(true);
    }

    protected void MoveTileUp()
    {
        var targetPosition = transform.position + new Vector3(0, TileManager.Instance.BuildSlotYOffset, 0);
        currentMoveUpCoroutine = StartCoroutine(TileManager.Instance.TileMoveCoroutine(transform, targetPosition));
    }

    protected void MoveTileDown() => moveToDefaultCoroutine =
        StartCoroutine(TileManager.Instance.TileMoveCoroutine(transform, defaultPosition));

    public void UnSelectTile()
    {
        MoveTileDown();
        tileCanMove = true;
    }

    public void SnapToDefaultPositionImmediately()
    {
        if (moveToDefaultCoroutine != null) StopCoroutine(moveToDefaultCoroutine);
        transform.position = defaultPosition;
    }

    public void SetSlotAvailable(bool value) => buildSlotAvailable = value;

    public Vector3 GetBuildPosition(float yOffset) => defaultPosition + new Vector3(0, yOffset);
}