using System;
using Unity.VisualScripting;
using UnityEngine;

public class Money : MonoBehaviour
{
    [field: SerializeField] public int CurrentBalance { get; private set; } = 800;

    public event Action<int> OnBalanceUpdate;

    private void Awake()
    {
        CurrentBalance = SaveAndLoad.LoadMoney();
        if (CurrentBalance <= 0)
        {
            CurrentBalance = 800;
        }
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentBalance += amount;
        OnBalanceUpdate?.Invoke(CurrentBalance);
        SaveAndLoad.SaveMoney(CurrentBalance);
    }

    public bool TryRemoveMoney(int amount) 
    {
        if (amount <= 0 || amount > CurrentBalance)
        {
            return false;
        }

        CurrentBalance -= amount;
        OnBalanceUpdate?.Invoke(CurrentBalance);
        SaveAndLoad.SaveMoney(CurrentBalance);
        return true;
    }
}
