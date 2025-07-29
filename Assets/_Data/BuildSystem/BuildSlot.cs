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
        if (!buildSlotAvailable || !tileCanMove || ManagerCtrl.Instance.TileManager.IsGridMoving) return;
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!tileCanMove || !buildSlotAvailable || ManagerCtrl.Instance.TileManager.IsGridMoving) return;

        if (currentMoveUpCoroutine != null) Invoke(nameof(MoveTileDown), ManagerCtrl.Instance.TileManager.DefaultMoveDuration);
        else
            MoveTileDown();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !buildSlotAvailable ||
            ManagerCtrl.Instance.BuildManager.SelectedBuildSlot == this || ManagerCtrl.Instance.TileManager.IsGridMoving ||
            !ManagerCtrl.Instance.GameManager.IsInGame) return;

        ManagerCtrl.Instance.BuildManager.EnableBuildMenu();
        ManagerCtrl.Instance.BuildManager.SelectBuildSlot(this);
        MoveTileUp();
        tileCanMove = false;
        ManagerCtrl.Instance.UI.InGameUI.BuildsBtnsUI.LastSelectedBtn?.SelectBtn(true);
    }

    protected void MoveTileUp()
    {
        var targetPosition = transform.position + new Vector3(0, ManagerCtrl.Instance.TileManager.BuildSlotYOffset, 0);
        currentMoveUpCoroutine = StartCoroutine(ManagerCtrl.Instance.TileManager.TileMoveCoroutine(transform, targetPosition));
    }

    protected void MoveTileDown() => moveToDefaultCoroutine =
        StartCoroutine(ManagerCtrl.Instance.TileManager.TileMoveCoroutine(transform, defaultPosition));

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