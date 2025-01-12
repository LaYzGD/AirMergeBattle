using System;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int CurrentBalance { get; private set; } = 400;

    public event Action<int> OnBalanceUpdate;

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentBalance += amount;
        OnBalanceUpdate?.Invoke(CurrentBalance);
    }

    public bool TryRemoveMoney(int amount) 
    {
        if (amount <= 0 || amount > CurrentBalance)
        {
            return false;
        }

        CurrentBalance -= amount;
        OnBalanceUpdate?.Invoke(CurrentBalance);
        return true;
    }
}
