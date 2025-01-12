using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class PurchaseHandler : MonoBehaviour
{
    [SerializeField] private int _boxPrice = 200;
    [SerializeField] private StatPrice<StatType>[] _statPrice; 

    private ItemBoxPool _boxPool;
    private Money _money;
    private GlobalStats _globalStats;

    public event Action<StatType, float> OnStatChanged;

    [Inject]
    public void Construct(ItemBoxPool boxPool, Money money, GlobalStats stats)
    {
        _boxPool = boxPool;
        _money = money;
        _globalStats = stats;
    }

    public void InitUpgradeInfo()
    {
        foreach (var stat in _statPrice)
        {
            OnStatChanged?.Invoke(stat.Type, stat.StatIncreasement);
        }
    }

    public bool TryBuyUpgrade(StatType type)
    {
        var item = _statPrice.FirstOrDefault(s => s.Type == type);

        if (item == null)
        {
            return false;
        }

        if (_money.TryRemoveMoney(item.Price))
        {
            _globalStats.IncreaseStat(item.Type, item.StatIncreasement);
            OnStatChanged?.Invoke(item.Type, item.StatIncreasement);
            return true;
        }

        return false;
    }

    public bool TryBuyBox()
    {
        if (_money.TryRemoveMoney(_boxPrice))
        {
            bool isBoxCreated = _boxPool.TryCreateBox();
            if (isBoxCreated) 
            {
                return true;
            }

            _money.AddMoney(_boxPrice);
            return false;
        }

        return false;
    }

    [Serializable]
    private class StatPrice<T>
    {
        [field: SerializeField] public T Type { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public float StatIncreasement { get; private set; }
    }
}

