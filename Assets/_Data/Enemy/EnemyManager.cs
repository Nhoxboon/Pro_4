using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : NhoxBehaviour
{
    [SerializeField] protected WaveDetails currentWave;

    [SerializeField] protected List<EnemyPortal> enemyPortals;

    protected override void Awake()
    {
        base.Awake();
        enemyPortals =
            new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
    }

    protected override void Start()
    {
        base.Start();
        SetNextWave();
    }

    [ContextMenu("Set Next Wave")]
    protected void SetNextWave()
    {
        List<string> newEnemyList = NewEnemyWave();
        int portalIndex = 0;

        for (int i = 0; i < newEnemyList.Count; i++)
        {
            string enemyNameToAdd = newEnemyList[i];
            EnemyPortal portalToReceiveEnemy = enemyPortals[portalIndex];

            portalToReceiveEnemy.GetEnemiesList().Add(enemyNameToAdd);
            portalIndex++;

            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
    }

    protected List<string> NewEnemyWave()
    {
        List<string> newEnemyList = new List<string>();

        foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
        {
            int count = currentWave.GetEnemyCount(enemyType);
            for (int i = 0; i < count; i++)
            {
                newEnemyList.Add(EnemySpawner.Instance.GetEnemyNameByType(enemyType));
            }
        }

        return newEnemyList;
    }
}