using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected Stat health;
    public Stat Health => health;


    [SerializeField] protected EntityStatsDataSO entityStatsDataSO;


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
        // if(transform.parent.parent.CompareTag("Player"))
        // {
        //     entityStatsDataSO = Resources.Load<EntityStatsDataSO>("Player/PlayerStats");
        // }
        // else
        // {
        //     entityStatsDataSO = Resources.Load<EntityStatsDataSO>("Enemies/Stats/" + transform.parent.parent.name + "Stats");
        // }
        // Debug.Log(transform.name + " LoadEntityStatsDataSO", gameObject);
    }
}