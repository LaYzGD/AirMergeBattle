using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class ItemBoxPool : MonoBehaviour
{
    [SerializeField] private float _silverDropChance = 0.1f;
    [SerializeField] private ItemBox _boxPrefab;
    [SerializeField] private ItemBoxType _wooden, _silver, _gold;

    private ObjectPool<ItemBox> _boxPool;
    private MergeGrid _grid;
    private GlobalStats _globalStats;

    [Inject]
    public void Construct(MergeGrid grid, GlobalStats stats) 
    {
        _grid = grid;
        _globalStats = stats;
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
        box.Initialize((Cell)validator.cell, isGolden ? _gold : (Random.Range(0f, 1f) < _silverDropChance ? _silver : _wooden), KillAction);
        validator.cell.SetItemFlag(true);
        return true;
    }

    public void IncreaseSilverBoxDropChance()
    {
        _silverDropChance = _globalStats.GetStat(StatType.SilverBoxDropChance).CurrentValue;
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
