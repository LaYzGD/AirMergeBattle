using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform _cellItemOrigin;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private CellItem _cellItem;
    private MergeGrid _mergeGrid;
    private InputReader _inputReader;
    private CellItemsPool _pool;

    public bool HasItem { get; private set; } = false;

    public void Init(CellItemsPool pool, MergeGrid grid, InputReader reader, Color color)
    {
        _spriteRenderer.color = color;
        _pool = pool;
        _mergeGrid = grid;
        _inputReader = reader;
    }

    public void CreateItem(TurretType type)
    {
        _cellItem = _pool.GetItem();
        _cellItem.transform.position = _cellItemOrigin.position;
        _cellItem.transform.SetParent(transform);
        _cellItem.Init(this, _mergeGrid, _inputReader, type);
        SetItemFlag(true);
    }

    public void SetItemFlag(bool flag)
    {
        HasItem = flag;
    }

    public bool CanPlaceOrMergeItem(CellItem item)
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
