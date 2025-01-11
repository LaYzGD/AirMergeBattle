using UnityEngine;

public class PlacementGrid : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [SerializeField] private GridData _data;
    [SerializeField] private PlacementCell _cellPrefab;

    private GridSystem<PlacementCell> _grid;

    private void Start()
    {
        _grid = new GridSystem<PlacementCell>(_data.Width, _data.Height, _data.CellSize, _origin.position);
        _grid.CreateGrid(null);
        InitializeGrid();
    }

    private void FillGridCell(Vector2 pos)
    {
        var cell = Instantiate(_cellPrefab);
        cell.transform.position = pos;
        cell.transform.SetParent(_origin);
        _grid.SetValue(pos, cell);
    }

    private void InitializeGrid()
    {
        foreach (var cellPos in _grid.GetPositions())
        {
            FillGridCell(cellPos);
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
}
