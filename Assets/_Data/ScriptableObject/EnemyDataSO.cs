using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "ScriptableObject/Enemy Data/Base Stats")]
public class EnemyDataSO : ScriptableObject
{
    public float health = 10f;
    public int threads = 10;
    public int currency = 10;
}