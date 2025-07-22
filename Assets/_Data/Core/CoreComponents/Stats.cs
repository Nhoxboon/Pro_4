using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected Stat health;
    public Stat Health => health;

    protected override void Awake()
    {
        base.Awake();

        health.SetMaxValue(10f);
        health.Init();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEntityStatsDataSO();
    }

    protected void LoadEntityStatsDataSO()
    {
        // if (entityStatsDataSO != null) return;
        // entityStatsDataSO = Resources.Load<EntityStatsDataSO>("Enemies/Stats/" + transform.parent.parent.name + "Stats");
        // DebugTool.Log(transform.name + " LoadEntityStatsDataSO", gameObject);
    }
}