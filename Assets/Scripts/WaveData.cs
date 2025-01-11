using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WaveData", fileName = "New WaveData")]
public class WaveData : ScriptableObject
{
    [field: SerializeField] public WaveInfo[] AllEnemies { get; private set; }
    [field: SerializeField] public int Reward { get; private set; } = 200;
    [field: SerializeField] public float DelayBetweenSpawn { get; private set; } = 5f;
}

[Serializable]
public struct WaveInfo 
{
    [field: SerializeField] public EnemyData EnemyType { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
}
