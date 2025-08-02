using System;
using UnityEngine;

public class SpiderLegRef : NhoxBehaviour
{
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float contactPointRadius = 0.05f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLayerMask();
    }

    protected void LoadLayerMask()
    {
        if(whatIsGround != 0) return;
        whatIsGround = LayerMask.GetMask("Default","Road");
        DebugTool.Log(transform.name + " :LoadLayerMask", gameObject);
    }

    public Vector3 ContactPoint()
    {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, whatIsGround)
            ? hitInfo.point
            : transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, ContactPoint());
        Gizmos.DrawWireSphere(ContactPoint(), contactPointRadius);
    }
}