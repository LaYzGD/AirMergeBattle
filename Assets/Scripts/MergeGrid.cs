using UnityEngine;

public class MergeGrid : MonoBehaviour
{
    [SerializeField] private Color _cellWhiteColor;
    [SerializeField] private Color _cellBlackColor;
    [SerializeField] private GridData _mergeGridData;
    [SerializeField] private Transform _origin;
    [SerializeField] private Cell _cellPrefab;

    private GridSystem<Cell> _grid;

    private void Start()
    {
        _grid = new GridSystem<Cell>(_mergeGridData.Width, _mergeGridData.Height, _mergeGridData.CellSize, _origin.position);
        _grid.CreateGrid(null);
        InitializeGrid();
    }

    private void FillGridCell(Vector2 pos, int counter) 
    {
        var cell = Instantiate(_cellPrefab);
        cell.transform.position = pos;
        cell.transform.SetParent(_origin);
        cell.transform.localScale = Vector2.one * _mergeGridData.CellSize;
        cell.Init(counter == 0 ? _cellWhiteColor : _cellBlackColor);
        _grid.SetValue(pos, cell);
    }

    private void InitializeGrid()
    {
        int counter = 0;

        foreach (var cellPos in _grid.GetPositions())
        {
            FillGridCell(cellPos, counter);
            counter++;
            if (counter > 1)
            {
                counter = 0;
            }
        }
    }

    public GridPlacementValidator ValidateDrop(Vector2 pos)
    {
        Vector2 cellPos;
        
        if (_grid.TryGetCoordinates(pos, out cellPos))
        {
            return new GridPlacementValidator(true, _grid.GetValue(cellPos), cellPos);
        }

        return new GridPlacementValidator(false, null, Vector2.zero);
    }

    public GridPlacementValidator ValidateBoxPlacement()
    {
        foreach (var pos in _grid.GetPositions())
        {
            var cell = _grid.GetValue(pos);
            if (!cell.HasItem)
            {
                return new GridPlacementValidator(true, cell, cell.transform.position);
            }
        }

        return new GridPlacementValidator(false, null, Vector2.zero);
    }

    public void CreateItem(TurretType type, Cell cell) 
    {
        cell.CreateItem(type);
    }
}

public struct GridPlacementValidator 
{
    public readonly bool flag;
    public readonly ICell cell;
    public readonly Vector2 cellPos;

    public GridPlacementValidator(bool flag, ICell cell, Vector2 cellPos)
    {
        this.flag = flag;
        this.cell = cell;
        this.cellPos = cellPos; 
    }
}
