using UnityEngine;

public class Waypoint : NhoxBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        GetComponent<MeshRenderer>().enabled = false;
    }
}