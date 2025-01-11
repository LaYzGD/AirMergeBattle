using UnityEngine;

public class PlacementCell : MonoBehaviour, ICell
{
    [SerializeField] private Transform _origin;
    private CellItem _currentItem;

    public bool HasItem => _currentItem != null;

    public void PlaceItem(CellItem item)
    {
        _currentItem = item;
        _currentItem.transform.position = _origin.position;
        _currentItem.transform.SetParent(transform);
        _currentItem.Activate(true);
    }

    public void RemoveItem()
    {
        _currentItem = null;
    }
}
