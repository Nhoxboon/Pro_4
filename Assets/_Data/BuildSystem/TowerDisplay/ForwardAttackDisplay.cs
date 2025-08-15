
using UnityEngine;

public class ForwardAttackDisplay : NhoxBehaviour
{
    [SerializeField] protected LineRenderer leftLine;
    [SerializeField] protected LineRenderer rightLine;
    [SerializeField] protected float attackRange;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLeftLine();
        LoadRightLine();
    }
    
    protected void LoadLeftLine()
    {
        if (leftLine != null) return;
        leftLine = transform.Find("LeftLine").GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " : LoadLeftLine", gameObject);
    }
    
    protected void LoadRightLine()
    {
        if (rightLine != null) return;
        rightLine = transform.Find("RightLine").GetComponent<LineRenderer>();
        DebugTool.Log(transform.name + " : LoadRightLine", gameObject);
    }

    public void CreateLines(bool showLines, float range)
    {
        leftLine.enabled = showLines;
        rightLine.enabled = showLines;
        
        if (!showLines) return;
        attackRange = range;
        UpdateLines();
    }
    
    public void UpdateLines()
    {
        if (!leftLine.enabled || !rightLine.enabled) return;
        
        DrawLine(leftLine);
        DrawLine(rightLine);
    }
    
    public void ChangeMaterial(Material newMaterial)
    {
        leftLine.material = newMaterial;
        rightLine.material = newMaterial;
    }

    protected void DrawLine(LineRenderer line)
    {
        Vector3 start = line.transform.position;
        Vector3 end = start + (transform.forward * attackRange);
        
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
