using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class ItemBoxPool : MonoBehaviour
{
    [SerializeField] private ItemBox _boxPrefab;
    [SerializeField] private ItemBoxType _wooden, _silver, _gold;

    private ObjectPool<ItemBox> _boxPool;
    private MergeGrid _grid;
    private GlobalStats _globalStats;

    private ItemBoxType[] _boxes;

    [Inject]
    public void Construct(MergeGrid grid, GlobalStats stats) 
    {
        _grid = grid;
        _globalStats = stats;
        _boxes = new ItemBoxType[] { _wooden, _silver, _gold };
    }

    private void Awake()
    {
        _boxPool = new ObjectPool<ItemBox>(OnCreate, OnGet, OnRelease, OnBoxDestroy, false);
    }

    public bool TryCreateBox(bool isGolden = false)
    {
        var validator = _grid.ValidateBoxPlacement();
        
        if (!validator.flag)
        {
            return false;
        }

        var box = _boxPool.Get();
        box.transform.position = validator.cellPos;
        var currentBox = _wooden;
        if (Random.Range(0f, 1f) < _globalStats.GetStat(StatType.SilverBoxDropChance).CurrentValue)
        {
            currentBox = _silver;
        }
        if (isGolden)
        {
            currentBox = _gold;
        }
        box.Initialize((Cell)validator.cell, currentBox, KillAction);
        validator.cell.SetItemFlag(true);
        SaveAndLoad.SaveCell(typeof(Cell), new CellInfo(true, validator.cell.Index, -1, currentBox.Index));
        return true;
    }

    public void CreateBox(Cell cell, int index)
    {
        var box = _boxPool.Get();
        box.transform.position = cell.transform.position;
        cell.SetItemFlag(true);
        box.Initialize(cell, _boxes.FirstOrDefault(b => b.Index == index), KillAction);
    }
    
    private void KillAction(ItemBox box)
    {
        _boxPool.Release(box);
    }

    private ItemBox OnCreate()
    {
        return Instantiate(_boxPrefab);
    }

    private void OnGet(ItemBox box)
    {
        box.gameObject.SetActive(true);
    }

    private void OnRelease(ItemBox box)
    {
        box.gameObject.SetActive(false);
    }

    private void OnBoxDestroy(ItemBox box)
    {
        Destroy(box.gameObject);
    }
}
