using UnityEngine;

public class MergeGrid : MonoBehaviour
{
    [SerializeField] private MergeGridData _mergeGridData;
    [SerializeField] private Transform _origin;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private InputReader _reader;
    [Space]
    [SerializeField] private CellItemsPool _cellItemsPool;

    private GridSystem<GridObject<Cell>> _grid;

    private void Start()
    {
        _grid = new GridSystem<GridObject<Cell>>(_mergeGridData.Width, _mergeGridData.Height, _mergeGridData.CellSize, _origin.position);
        _grid.CreateGrid(null);
        InitializeGrid();
    }

    private void FillGridCell(Vector2 pos, int counter) 
    {
        var cell = Instantiate(_cellPrefab);
        cell.transform.position = pos;
        cell.transform.SetParent(_origin);
        cell.transform.localScale = Vector2.one * _mergeGridData.CellSize;
        cell.Init(_cellItemsPool, this, _reader, counter == 0 ? _mergeGridData.CellWhiteColor : _mergeGridData.CellBlackColor);
        var gridObject = new GridObject<Cell>(pos);
        gridObject.SetValue(cell);
        _grid.SetValue(pos, gridObject);
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
            return new GridPlacementValidator(true, _grid.GetValue(cellPos).Object);
        }

        return new GridPlacementValidator(false, null);
    }

    public GridPlacementValidator ValidateBoxPlacement()
    {
        foreach (var pos in _grid.GetPositions())
        {
            var cell = _grid.GetValue(pos).Object;
            if (!cell.HasItem)
            {
                return new GridPlacementValidator(true, cell);
            }
        }

        return new GridPlacementValidator(false, null);
    }

    public void CreateItem(TurretType type, Cell cell) 
    {
        cell.CreateItem(type);
    }
}

public struct GridPlacementValidator 
{
    public readonly bool flag;
    public readonly Cell cell;

    public GridPlacementValidator(bool flag, Cell cell)
    {
        this.flag = flag;
        this.cell = cell;
    }
}
