using UnityEngine;
using Zenject;

public class CellItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private MergeGrid _mergeGrid;
    private PlacementGrid _placementGrid;
    private InputReader _inputReader;

    private ICell _currentCell;

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
    }

    private void OnMouseDown()
    {
        _currentCell.RemoveItem();
        transform.SetParent(null);
    }

    private void OnMouseDrag()
    {
        transform.position = _inputReader.MousePosition;
    }

    private void OnMouseUp() 
    {
        var placementValidator = _placementGrid.ValidateDrop(_inputReader.MousePosition);

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
}
