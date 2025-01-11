using UnityEngine;
using Zenject;

public class Cell : MonoBehaviour, ICell
{
    [SerializeField] private Transform _cellItemOrigin;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private CellItem _cellItem;
    private CellItemsPool _pool;

    public bool HasItem { get; private set; } = false;

    [Inject]
    public void Construct(CellItemsPool pool) 
    {
        _pool = pool;
    }

    public void Init(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void CreateItem(TurretType type)
    {
        _cellItem = _pool.GetItem();
        _cellItem.transform.position = _cellItemOrigin.position;
        _cellItem.transform.SetParent(transform);
        _cellItem.Init(this, type);
        SetItemFlag(true);
    }

    public void SetItemFlag(bool flag)
    {
        HasItem = flag;
    }

    public bool CanPlaceItem(CellItem item)
    {
        if (_cellItem != null && (_cellItem.TurretType != item.TurretType || _cellItem.TurretType.NextUpgrade == null))
        {
            return false;
        }

        return true;
    }

    public void PlaceItem(CellItem item)
    {
        if (_cellItem == null) 
        {
            _cellItem = item;
            _cellItem.transform.position = _cellItemOrigin.position;
            _cellItem.transform.SetParent(transform);
            SetItemFlag(true);
            return;
        }

        _pool.RemoveItem(_cellItem);
        MergeItems(item);
    }

    public void RemoveItem()
    {
        _cellItem = null;
        SetItemFlag(false);
    }

    private void MergeItems(CellItem item)
    {
        _cellItem = item;
        var nextUpgrade = _cellItem.TurretType.NextUpgrade;
        _cellItem.transform.position = _cellItemOrigin.position;
        _cellItem.transform.SetParent(transform);
        _cellItem.SetType(nextUpgrade);
        SetItemFlag(true);
    }
}