using UnityEngine;

public class PlacementCell : MonoBehaviour, ICell
{
    [SerializeField] private Transform _origin;
    public int Index { get; private set; }
    private CellItem _currentItem;

    public bool HasItem => _currentItem != null;

    public void Init(int index)
    {
        Index = index;
    }

    public void PlaceItem(CellItem item)
    {
        _currentItem = item;
        _currentItem.transform.position = _origin.position;
        _currentItem.transform.SetParent(transform);
        _currentItem.Activate(true);
        SaveAndLoad.SaveCell(typeof(PlacementCell), new CellInfo(HasItem, Index, _currentItem.TurretType.Index));
    }

    public void RemoveItem()
    {
        _currentItem = null;
        SaveAndLoad.SaveCell(typeof(PlacementCell), new CellInfo(HasItem, Index));
    }
}
