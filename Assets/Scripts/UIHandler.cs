using UnityEngine;
using Zenject;

public class UIHandler : MonoBehaviour
{
    private ItemBoxPool _itemBoxPool;
    private Money _money;

    [Inject]
    public void Construct(ItemBoxPool boxPool, Money money)
    {
        _itemBoxPool = boxPool;
        _money = money;
    }

    public void OnCreateBoxButtonClick() 
    {
        var isBoxCreated = _itemBoxPool.TryCreateBox();
        if (!isBoxCreated)
        {
            return;
        }
    }
}
