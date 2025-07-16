using System.Collections;
using UnityEngine;

public class TileManager : NhoxBehaviour
{
    private static TileManager instance;
    public static TileManager Instance => instance;

    [SerializeField] protected float yMovementDuration = 0.1f;
    public float YMovementDuration => yMovementDuration;

    [Header("Build Slot Movement")] [SerializeField]
    protected float buildSlotYOffset = 0.25f;

    public float BuildSlotYOffset => buildSlotYOffset;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one TileManager allowed to exist");
            return;
        }

        instance = this;
    }

    public void MoveTile(Transform objectToMove, Vector3 targetPosition) =>
        StartCoroutine(TileMoveCoroutine(objectToMove, targetPosition));

    public IEnumerator TileMoveCoroutine(Transform objectToMove, Vector3 targetPosition)
    {
        float time = 0f;
        Vector3 startPosition = objectToMove.position;

        while (time < yMovementDuration)
        {
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, time / yMovementDuration);
            time += Time.deltaTime;
            yield return null;
        }

        objectToMove.position = targetPosition;
    }
}