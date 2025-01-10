using UnityEngine;
using UnityEngine.Pool;

public class CellItemsPool : MonoBehaviour
{
    [SerializeField] private CellItem _cellItemPrefab;
    private ObjectPool<CellItem> _itemsPool;

    private void Awake()
    {
        _itemsPool = new ObjectPool<CellItem>(OnCreate, OnGet, OnRelease, OnItemDestroy, false);
    }

    public CellItem GetItem()
    {
        return _itemsPool.Get();
    }

    public void RemoveItem(CellItem item) 
    {
        _itemsPool.Release(item);
    }

    private CellItem OnCreate()
    {
        return Instantiate(_cellItemPrefab);
    }

    private void OnGet(CellItem item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnRelease(CellItem item) 
    {
        item.gameObject.SetActive(false);
    }

    private void OnItemDestroy(CellItem item)
    {
        Destroy(item.gameObject);
    }
}
