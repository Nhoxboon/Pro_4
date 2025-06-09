using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveDetails
{
    [SerializeField] private List<EnemyTypeCount> enemyTypes = new List<EnemyTypeCount>();

    public int GetEnemyCount(EnemyType enemyType)
    {
        var typeCount = enemyTypes.Find(x => x.enemyType == enemyType);
        return typeCount?.count ?? 0;
    }
}