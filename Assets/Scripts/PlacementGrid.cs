using UnityEngine;
using Zenject;

public class PlacementGrid : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [SerializeField] private GridData _data;
    [SerializeField] private PlacementCell _cellPrefab;

    private GridSystem<PlacementCell> _grid;
    private CellItemsPool _pool;
    private AllTurretUpgrades _allTurretUpgrades;

    [Inject]
    public void Construct(CellItemsPool pool, AllTurretUpgrades upgrades)
    {
        _pool = pool;
        _allTurretUpgrades = upgrades;
    }

    private void Start()
    {
        _grid = new GridSystem<PlacementCell>(_data.Width, _data.Height, _data.CellSize, _origin.position);
        _grid.CreateGrid(null);
        InitializeGrid();
    }

    private void FillGridCell(Vector2 pos, int index)
    {
        var cell = Instantiate(_cellPrefab);
        cell.transform.position = pos;
        cell.transform.SetParent(_origin);
        cell.Init(index);
        var cellInfo = SaveAndLoad.LoadCell(index, typeof(PlacementCell));
        if (cellInfo != null && cellInfo.HasItem)
        {
            var item = _pool.GetItem();
            item.Init(cell, _allTurretUpgrades.GetTurretByIndex(cellInfo.TurretIndex));
            cell.PlaceItem(item);
        }
        _grid.SetValue(pos, cell);
    }

    private void InitializeGrid()
    {
        int index = 0;
        foreach (var cellPos in _grid.GetPositions())
        {
            FillGridCell(cellPos, index);
            index++;
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
