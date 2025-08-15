using System;
using System.Collections.Generic;
using UnityEngine;

public class Visuals : CoreComponent
{
    [SerializeField] protected Transform visuals;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float verticalRotationSpeed = 5f;

    [Header("Transparency Details")] 
    [SerializeField] protected Material transparentMat;
    protected List<Material> originalMat;
    [SerializeField] protected MeshRenderer[] myRenderers;

    protected override void Awake()
    {
        base.Awake();
        CollectDefaultMaterials();
    }

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
        LoadMaterials();
        LoadMyRenderers();
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
    
    protected void LoadMaterials()
    {
        if (transparentMat != null) return;
        transparentMat = Resources.Load<Material>("Materials/Enemy_Transparent");
        DebugTool.Log(transform.name + " :LoadMaterials", gameObject);
    }

    protected void LoadMyRenderers()
    {
        if (myRenderers is { Length: > 0 }) return;
        myRenderers = visuals.GetComponentsInChildren<MeshRenderer>();
        DebugTool.Log(transform.name + " :LoadMyRenderers", gameObject);
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

    protected void CollectDefaultMaterials()
    {
        originalMat = new List<Material>();
        foreach (MeshRenderer mesh in myRenderers)
            originalMat.Add(mesh.material);
    }

    public void MakeTransparent(bool transparent)
    {
        if (originalMat == null)
            CollectDefaultMaterials();
        
        for (int i = 0; i < myRenderers.Length; i++)
            myRenderers[i].material = transparent ? transparentMat : originalMat?[i];
    }

    public virtual void ResetVisuals()
    {
        MakeTransparent(false);
        AlignWithSlope();
    }
}