using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreComponent : NhoxBehaviour
{
    [Header("Core Component")] [SerializeField]
    protected Core core;

    protected override void Awake()
    {
        base.Awake();
        core.AddComponent(this);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
    }

    protected void LoadCore()
    {
        if (core != null) return;
        core = transform.parent.GetComponent<Core>();
        DebugTool.Log(transform.name + " LoadCore", gameObject);
    }

    public virtual void LogicUpdate()
    {
        //For override
    }
}