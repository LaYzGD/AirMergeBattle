using UnityEngine;

public class Money : MonoBehaviour
{
    public int CurrentBalance { get; private set; }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentBalance += amount;
    }

    public void RemoveMoney(int amount) 
    {
        if (amount <= 0 || amount > CurrentBalance)
        {
            return;
        }

        CurrentBalance -= amount;
    }
}
