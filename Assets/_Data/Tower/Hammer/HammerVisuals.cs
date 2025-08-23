using System;
using System.Collections;
using UnityEngine;

public class HammerVisuals : TowerVisuals
{
    [SerializeField] protected RotateObject valveRotation;
    protected string hammerVFX = "ExplosionFX_1";
    protected string hammerAttackVFX = "RippleFX";

    [Header("Hammer Visual")] [SerializeField]
    protected Transform hammer;

    [SerializeField] protected Transform hammerHolder;
    [SerializeField] protected Transform sideWire;
    [SerializeField] protected Transform sideHandle;
    
    protected Vector3 hammerOriginalPosition;
    protected Vector3 sideHandleOriginalPosition;

    [Header("Attack and Release Details")] [SerializeField]
    protected float attackYOffset = 0.55f;

    [SerializeField] protected float attackDuration = 0.1f;
    protected float reloadDuration = 2f;

    protected override void Awake()
    {
        base.Awake();
        reloadDuration = towerCtrl.Attack.AttackCooldown - attackDuration;
        hammerOriginalPosition = hammer.localPosition;
        sideHandleOriginalPosition = sideHandle.localPosition;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadValveRotation();
        LoadHammer();
        LoadHammerHolder();
        LoadSideWire();
        LoadSideHandle();
    }

    protected void LoadValveRotation()
    {
        if (valveRotation != null) return;
        valveRotation = towerCtrl.GetComponentInChildren<RotateObject>();
        DebugTool.Log(transform.name + " :LoadValveRotation", gameObject);
    }

    protected void LoadHammer()
    {
        if (hammer != null) return;
        hammer = transform.parent.parent.Find("Model/Hammer");
        DebugTool.Log(transform.name + " :LoadHammer", gameObject);
    }

    protected void LoadHammerHolder()
    {
        if (hammerHolder != null) return;
        hammerHolder = transform.parent.parent.Find("Model/HammerHolder");
        DebugTool.Log(transform.name + " :LoadHammerHolder", gameObject);
    }

    protected void LoadSideWire()
    {
        if (sideWire != null) return;
        sideWire = transform.parent.parent.Find("Model/SideWire");
        DebugTool.Log(transform.name + " :LoadSideWire", gameObject);
    }

    protected void LoadSideHandle()
    {
        if (sideHandle != null) return;
        sideHandle = transform.parent.parent.Find("Model/SideHandle");
        DebugTool.Log(transform.name + " :LoadSideHandle", gameObject);
    }

    public void HammerAttackVFX()
    {
        StopAllCoroutines();
        StartCoroutine(HammerAttackCoroutine());
    }

    protected IEnumerator HammerAttackCoroutine()
    {
        valveRotation.SetRotationSpeed(25);
        StartCoroutine(ChangePositionCoroutine(hammer, -attackYOffset, attackDuration));
        StartCoroutine(ChangeScaleCoroutine(hammerHolder, 7f, attackDuration));

        StartCoroutine(ChangePositionCoroutine(sideHandle, 0.45f, attackDuration));
        StartCoroutine(ChangeScaleCoroutine(sideWire, 0.1f, attackDuration));

        yield return new WaitForSeconds(attackDuration);
        PlayHammerVFX();

        valveRotation.SetRotationSpeed(3);
        StartCoroutine(ChangePositionCoroutine(hammer, attackYOffset, reloadDuration));
        StartCoroutine(ChangeScaleCoroutine(hammerHolder, 1f, reloadDuration));

        StartCoroutine(ChangePositionCoroutine(sideHandle, -0.45f, reloadDuration));
        StartCoroutine(ChangeScaleCoroutine(sideWire, 1f, reloadDuration));
    }

    protected IEnumerator ChangePositionCoroutine(Transform transform, float yOffset, float duration = 0.1f)
    {
        float time = 0f;
        Vector3 initialPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);

        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
    }

    protected void PlayHammerVFX()
    {
        SpawnVFX(hammerAttackVFX, hammer.position, Quaternion.identity);
        SpawnVFX(hammerVFX, hammer.position, Quaternion.identity);
    }

    public override void ResetVisual()
    {
        base.ResetVisual();
        if (hammerOriginalPosition != Vector3.zero)
            hammer.localPosition = hammerOriginalPosition;

        hammerHolder.localScale = Vector3.one;
        sideWire.localScale = Vector3.one;

        if (sideHandleOriginalPosition != Vector3.zero)
            sideHandle.localPosition = sideHandleOriginalPosition;
        valveRotation.SetRotationSpeed(3);
    }
}