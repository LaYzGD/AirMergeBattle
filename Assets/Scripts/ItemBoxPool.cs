using UnityEngine;
using UnityEngine.Pool;

public class ItemBoxPool : MonoBehaviour
{
    [SerializeField] private MergeGrid _grid;
    [SerializeField] private ItemBox _boxPrefab;
    [SerializeField] private ItemBoxType _wooden, _silver, _gold;

    private ObjectPool<ItemBox> _boxPool;

    private float _silverDropChance = 0f;

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
        box.transform.position = validator.cell.transform.position;
        box.Initialize(_grid, validator.cell, isGolden ? _gold : (Random.Range(0f, 1f) < _silverDropChance ? _silver : _wooden), KillAction);
        validator.cell.SetItemFlag(true);
        return true;
    }

    public void IncreaseSilverBoxDropChance(float value)
    {
        if (value <= 0) return;

        _silverDropChance += value;
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
