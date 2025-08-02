using System;
using System.Collections;
using UnityEngine;

public class SpiderLeg : NhoxBehaviour
{
    [SerializeField] protected SpiderVisuals spiderVisuals;
    [SerializeField] protected float legSpeed = 2.5f;
    [SerializeField] protected float moveThreshold = 0.45f;
    protected bool shouldMove;
    protected bool canMove = true;

    [Header("Leg Settings")] [SerializeField]
    protected Vector3 placementOffset;

    [SerializeField] protected SpiderLeg oppositeLeg;
    [SerializeField] protected SpiderLegRef legRef;
    [SerializeField] protected Transform actualTarget;
    [SerializeField] protected Transform bottomLeg;
    protected Transform worldTargetRef;

    protected Coroutine moveCoroutine;

    protected override void Awake()
    {
        base.Awake();
        CreateWorldTargetRef();
        legSpeed = spiderVisuals.legSpeed;
    }

    protected void CreateWorldTargetRef()
    {
        var refPointObj = new GameObject("Ref_Point");
        worldTargetRef = Instantiate(refPointObj, actualTarget.position, Quaternion.identity).transform;
        worldTargetRef.gameObject.name = legRef.gameObject.name + "_World";
        Destroy(refPointObj);
    }

    public void UpdateLegMovement()
    {
        actualTarget.position = worldTargetRef.position + placementOffset;
        shouldMove = Vector3.Distance(worldTargetRef.position, legRef.ContactPoint()) > moveThreshold;

        if (bottomLeg is not null)
            bottomLeg.forward = Vector3.down;

        if (!shouldMove || !canMove) return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(LegMoveCoroutine());
    }

    protected IEnumerator LegMoveCoroutine()
    {
        oppositeLeg.CanMove(false);
        while (Vector3.Distance(worldTargetRef.position, legRef.ContactPoint()) > 0.01f)
        {
            worldTargetRef.position =
                Vector3.MoveTowards(worldTargetRef.position, legRef.ContactPoint(), legSpeed * Time.deltaTime);
            yield return null;
        }

        oppositeLeg.CanMove(true);
    }

    public void CanMove(bool enable) => canMove = enable;

    public void SetSpiderVisuals(SpiderVisuals visuals) => spiderVisuals = visuals;
}