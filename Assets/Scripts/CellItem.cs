using UnityEngine;

public class CellItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private MergeGrid _mergeGrid;
    private InputReader _inputReader;

    private Cell _currentCell;

    public TurretType TurretType { get; private set; }

    public void Init(Cell cell, MergeGrid mergeGrid, InputReader reader, TurretType type)
    {
        _currentCell = cell;
        _mergeGrid = mergeGrid;
        _inputReader = reader;
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
        var validator = _mergeGrid.ValidateDrop(_inputReader.MousePosition);
        
        if (!validator.flag || !validator.cell.CanPlaceOrMergeItem(this))
        {
            _currentCell.PlaceItem(this);
            return;
        }

        _currentCell = validator.cell;
        _currentCell.PlaceItem(this);
    }
}
