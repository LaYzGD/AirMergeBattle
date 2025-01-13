using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
public class CellItem : MonoBehaviour, IDragable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ItemShoot _itemShoot;

    private MergeGrid _mergeGrid;
    private PlacementGrid _placementGrid;
    private InputReader _inputReader;
    private ICell _currentCell;

    private bool _isDragging;

    public TurretType TurretType { get; private set; }

    [Inject]
    public void Construct(MergeGrid mergeGrid, InputReader reader, PlacementGrid placementGrid)
    {
        _mergeGrid = mergeGrid;
        _inputReader = reader;
        _placementGrid = placementGrid;
    }

    public void Init(ICell cell, TurretType type)
    {
        _currentCell = cell;
        SetType(type);
    }

    public void SetType(TurretType type)
    {
        TurretType = type;
        _spriteRenderer.sprite = TurretType.Sprite;
        if (_itemShoot != null)
        {
            _itemShoot.Init(type);
        }
    }

    public void OnDragStart()
    {
        _currentCell.RemoveItem();
        Activate(false);
        transform.SetParent(null);
        _isDragging = true;
    }

    public void OnDrag()
    {
        transform.position = _inputReader.MousePosition;
    }

    private void Update()
    {
        if (_isDragging)
        {
            OnDrag();
        }
    }

    public void OnDragEnd() 
    {
        if (!_isDragging)
        {
            return;
        }

        var placementValidator = _placementGrid.ValidateDrop(_inputReader.MousePosition);
        _isDragging = false;

        if (placementValidator.flag)
        {
            var cell = placementValidator.cell;

            if (cell.HasItem)
            {
                _currentCell.PlaceItem(this);
                return;
            }

            cell.PlaceItem(this);
            _currentCell = cell;
            return;
        }

        var mergeValidator = _mergeGrid.ValidateDrop(_inputReader.MousePosition);
        
        if (!mergeValidator.flag || !mergeValidator.cell.CanPlaceItem(this))
        {
            _currentCell.PlaceItem(this);
            return;
        }

        _currentCell = mergeValidator.cell;
        _currentCell.PlaceItem(this);
    }

    public void Activate(bool flag) 
    {
        if (_itemShoot == null)
        {
            return;
        }   

        if (flag) 
        {
            _itemShoot.StartShooting();
            return;
        }

        _itemShoot.StopShooting();
    }
}
