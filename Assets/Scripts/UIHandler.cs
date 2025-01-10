using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private ItemBoxPool _itemBoxPool;
    [SerializeField] private Money _money;

    public void OnCreateBoxButtonClick() 
    {
        var isBoxCreated = _itemBoxPool.TryCreateBox();
        if (!isBoxCreated)
        {
            return;
        }
    }
}
