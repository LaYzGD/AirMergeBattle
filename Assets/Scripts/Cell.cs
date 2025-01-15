using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
public class Cell : MonoBehaviour, ICell
{
    [SerializeField] private Transform _cellItemOrigin;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _mergeSound;
    [SerializeField] private float _mergeSoundVolume;
    [SerializeField] private VFXObjectData _vfxData;

    private CellItem _cellItem;
    private CellItemsPool _itemsPool;
    private AudioPlayer _audioPlayer;
    private VFXPool _vFXPool;

    public int Index { get; private set; }
    public CellItem CellItem => _cellItem;

    public bool HasItem { get; private set; } = false;

    [Inject]
    public void Construct(CellItemsPool itemsPool, AudioPlayer audioPlayer, VFXPool vfxPool) 
    {
        _itemsPool = itemsPool;
        _audioPlayer = audioPlayer;
        _vFXPool = vfxPool;
    }

    public void Init(Color color, int index)
    {
        _spriteRenderer.color = color;
        Index = index;
    }

    public void CreateItem(TurretType type)
    {
        _cellItem = _itemsPool.GetItem();
        _cellItem.transform.position = _cellItemOrigin.position;
        _cellItem.transform.SetParent(transform);
        _cellItem.Init(this, type);
        SetItemFlag(true);
        SaveAndLoad.SaveCell(typeof(Cell), new CellInfo(HasItem, Index, _cellItem.TurretType.Index));
    }

    public void SetItemFlag(bool flag)
    {
        HasItem = flag;
    }

    public bool CanPlaceItem(CellItem item)
    {
        if (_cellItem != null && (_cellItem.TurretType != item.TurretType || _cellItem.TurretType.NextUpgrade == null))
        {
            return false;
        }

        return true;
    }

    public void PlaceItem(CellItem item)
    {
        if (_cellItem == null) 
        {
            _cellItem = item;
            _cellItem.transform.position = _cellItemOrigin.position;
            _cellItem.transform.SetParent(transform);
            SetItemFlag(true);
            SaveAndLoad.SaveCell(typeof(Cell), new CellInfo(HasItem, Index, _cellItem.TurretType.Index));
            return;
        }

        _itemsPool.RemoveItem(_cellItem);
        MergeItems(item);
    }

    public void RemoveItem()
    {
        _cellItem = null;
        SetItemFlag(false);
        SaveAndLoad.SaveCell(typeof(Cell), new CellInfo(false, Index));
    }

    private void MergeItems(CellItem item)
    {
        _cellItem = item;
        var nextUpgrade = _cellItem.TurretType.NextUpgrade;
        _cellItem.transform.position = _cellItemOrigin.position;
        _vFXPool.SpawnVFX(_vfxData, _cellItemOrigin.position);
        _cellItem.transform.SetParent(transform);
        _cellItem.SetType(nextUpgrade);
        SetItemFlag(true);
        _audioPlayer.PlaySound(_mergeSound, _mergeSoundVolume);
        SaveAndLoad.SaveCell(typeof(Cell), new CellInfo(HasItem, Index, _cellItem.TurretType.Index));
    }
}
