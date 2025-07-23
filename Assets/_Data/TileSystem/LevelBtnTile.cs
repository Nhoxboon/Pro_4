using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBtnTile : NhoxBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected int levelIndex;
    protected bool canClick;
    protected bool canMove;
    
    protected Vector3 defaultPosition;
    protected Coroutine currentMoveCoroutine;
    protected Coroutine moveToDefaultCoroutine;

    protected void OnEnable() => canMove = true;

    protected override void Awake()
    {
        base.Awake();
        defaultPosition = transform.position;
    }

    public void EnableClick(bool enable) => canClick = enable;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick) return;
        Debug.Log("Loading level_" + levelIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!canMove) return;
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!canMove) return;
        
        if (currentMoveCoroutine != null)
            Invoke(nameof(MoveToDefaultPosition), TileManager.Instance.DefaultMoveDuration);
        else
            MoveToDefaultPosition();
    }

    protected void MoveTileUp()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, TileManager.Instance.BuildSlotYOffset, 0);
        currentMoveCoroutine = StartCoroutine(TileManager.Instance.TileMoveCoroutine(transform, targetPosition));
    }
    
    protected void MoveToDefaultPosition()
    {
        moveToDefaultCoroutine = StartCoroutine(TileManager.Instance.TileMoveCoroutine(transform, defaultPosition));
    }
}