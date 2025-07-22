
using UnityEngine;

public class WaypointManager : NhoxBehaviour
{
    private static WaypointManager instance;
    public static WaypointManager Instance => instance;
    
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            DebugTool.LogError("Only one WaypointManager instance allow to exist.");
            return;
        }
        instance = this;
    }
    
    [SerializeField] protected Transform[] waypoints;
    public Transform[] Waypoints => waypoints;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWaypoints();
    }
    
    protected void LoadWaypoints()
    {
        if (waypoints.Length > 0) return;
    
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
        DebugTool.Log($"Loaded {waypoints.Length} waypoints");
    }
}
