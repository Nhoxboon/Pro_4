using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TowerAtkRadiusDisplay : NhoxBehaviour
{
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected float lineWidth = 0.1f;
    protected int segments = 50;

    protected override void Awake()
    {
        base.Awake();
        SetupLineRenderer();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLineRenderer();
    }

    protected void LoadLineRenderer()
    {
        if (lineRenderer != null) return;
        lineRenderer = GetComponent<LineRenderer>();
        // Debug.Log(transform.name + ": LoadLineRenderer", gameObject);
    }

    protected void SetupLineRenderer()
    {
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.material = FindFirstObjectByType<BuildManager>().AttackRadMat;
    }

    public void CreateCircle(bool showRadius, float radius = 0f)
    {
        lineRenderer.enabled = showRadius;
        if (!showRadius) return;

        float angle = 0f;
        Vector3 center = transform.position;

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x + center.x, center.y, z + center.z));
            angle += 360f / segments;
        }

        lineRenderer.SetPosition(segments, lineRenderer.GetPosition(0));
    }
}