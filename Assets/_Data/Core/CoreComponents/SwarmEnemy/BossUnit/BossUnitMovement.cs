using System;
using System.Collections;
using UnityEngine;

public class BossUnitMovement : Movement
{
    [Header("Boss Unit Movement")] [SerializeField]
    protected Rigidbody rb;

    protected Vector3 savedDestination;
    protected Vector3 lastKnownBossPosition;
    protected Enemy myBoss;

    protected Coroutine snapCoroutine;

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(myBoss is not null)
            lastKnownBossPosition = myBoss.transform.position;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = core.Root.GetComponent<Rigidbody>();
        DebugTool.Log(transform.name + " :LoadRigidbody", gameObject);
    }

    public void SetUpBossUnit(Vector3 destination, Enemy myNewBoss, EnemyPortal myNewPortal)
    {
        ResetMovement();

        myBoss = myNewBoss;
        core.Enemy.SetPortal(myNewPortal);
        core.Enemy.MyPortal.GetActiveEnemies().Add(core.Root.gameObject);
        savedDestination = destination;

        if (snapCoroutine != null) StopCoroutine(snapCoroutine);
        snapCoroutine = StartCoroutine(SnapToBossRoutine());
    }

    public void HandleUnitCollision()
    {
        if (Vector3.Distance(core.Root.transform.position, lastKnownBossPosition) > 2.5f)
                core.Root.transform.position = lastKnownBossPosition + Vector3.down;
        rb.useGravity = false;
        rb.isKinematic = true;

        agent.enabled = true;
        agent.SetDestination(savedDestination);
    }

    private IEnumerator SnapToBossRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        SnapToBoss();
        yield return new WaitForSeconds(0.5f);
    }

    protected void SnapToBoss()
    {
        if (!agent.enabled || agent.isOnNavMesh) return;
        if (!(Vector3.Distance(core.Root.transform.position, lastKnownBossPosition) > 3f)) return;
        core.Root.transform.position = lastKnownBossPosition + Vector3.down;
        ResetMovement();
    }

    public override void ResetMovement()
    {
        base.ResetMovement();
        rb.useGravity = true;
        rb.isKinematic = false;
        agent.enabled = false;
    }
}