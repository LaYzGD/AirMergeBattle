using System;
using System.Linq;
using UnityEngine;

public class GlobalStats : MonoBehaviour
{
    [SerializeField] private Stat[] _allStats;

    public void SetUp()
    {
        foreach (Stat stat in _allStats) 
        {
            stat.Set(stat.BaseValue);
        }
    }

    public void IncreaseStat(StatType type, float value)
    {
        if (value <= 0) return;

        var stat = _allStats.FirstOrDefault(s => s.Type == type);

        if (stat == null)
        {
            return;
        }

        float newValue = stat.CurrentValue + value;

        if (stat.MaxValue != -1f)
        {
            newValue = Mathf.Clamp(stat.CurrentValue + value, 0f, stat.MaxValue);
        }

        stat.Set(newValue);
    }

    public Stat GetStat(StatType type) 
    {
        var stat = _allStats.FirstOrDefault(s => s.Type == type);

        return stat;
    }
}

public enum StatType 
{
    SilverBoxDropChance = 0,
    BaseHealth = 1,
    StructureDamage = 2,
    ProjectileSpeed = 3,
    StructureDelay = 4
}


[Serializable]
public class Stat
{
    [field: SerializeField] public float MaxValue { get; private set; } = -1f;
    [field: SerializeField] public float BaseValue { get; private set; }
    [field: SerializeField] public StatType Type { get; private set; }
    public float CurrentValue { get; private set; }

    public void Set(float value) 
    {
        CurrentValue = value;
    }
}
