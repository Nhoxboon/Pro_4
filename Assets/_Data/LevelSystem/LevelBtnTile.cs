using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBtnTile : NhoxBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected TextMeshPro myText;
    [SerializeField] protected int levelIndex;
    protected bool canClick;
    protected bool unlocked;

    protected Vector3 defaultPosition;
    protected Coroutine currentMoveCoroutine;
    protected Coroutine moveToDefaultCoroutine;

    protected override void Awake()
    {
        base.Awake();
        defaultPosition = transform.position;
        CheckIfLevelUnlocked();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadText();
    }

    protected void LoadText()
    {
        if (myText != null) return;
        myText = GetComponentInChildren<TextMeshPro>();
        DebugTool.Log(transform.name + " :LoadText", gameObject);
    }

    public void EnableClick(bool enable) => canClick = enable;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick || !unlocked) return;

        transform.position = defaultPosition;
        LevelManager.Instance.LoadLevelFromMenu("Level_" + levelIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TileManager.Instance.IsGridMoving) return;
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TileManager.Instance.IsGridMoving) return;

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

    protected void MoveToDefaultPosition() => moveToDefaultCoroutine =
        StartCoroutine(TileManager.Instance.TileMoveCoroutine(transform, defaultPosition));

    public void CheckIfLevelUnlocked()
    {
        if (levelIndex == 1)
            PlayerPrefs.SetInt("Level_1" + "unlocked", 1);
        unlocked = PlayerPrefs.GetInt("Level_" + levelIndex + "unlocked", 0) == 1;

        if (!unlocked)
            myText.text = "Locked";
        else
            myText.text = "Level " + levelIndex;
    }

    protected void OnValidate()
    {
        levelIndex = transform.GetSiblingIndex() + 1;

        if (myText != null) myText.text = "Level " + levelIndex;
    }
}