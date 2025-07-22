using System;
using UnityEngine;

public class Visuals : CoreComponent
{
    [SerializeField] protected Transform visuals;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float verticalRotationSpeed = 5f;

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        AlignWithSlope();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadVisuals();
        LoadLayerMask();
    }

    protected void LoadVisuals()
    {
        if (visuals != null) return;
        visuals = core.Root.transform.Find("Model");
        DebugTool.Log(transform.name + " :LoadVisuals", gameObject);
    }

    protected void LoadLayerMask()
    {
        if (whatIsGround != 0) return;
        whatIsGround = LayerMask.GetMask("Road");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    protected void AlignWithSlope()
    {
        if (visuals is null) return;
        if (Physics.Raycast(visuals.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, whatIsGround))
        {
            Quaternion targetRotation =
                Quaternion.FromToRotation(transform.parent.up, hit.normal) * transform.parent.rotation;
            visuals.rotation =
                Quaternion.Slerp(visuals.rotation, targetRotation, verticalRotationSpeed * Time.deltaTime);
        }
    }
}