using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
public class ItemBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private ItemBoxType _itemBoxType;
    private MergeGrid _mergeGrid;
    private Action<ItemBox> _onDestroy;
    private ICell _cell;

    [Inject]
    public void Construct(MergeGrid grid)
    {
        _mergeGrid = grid;
    }

    public void Initialize(Cell cell, ItemBoxType boxType, Action<ItemBox> killAction)
    {
        _cell = cell;
        _itemBoxType = boxType;
        _spriteRenderer.sprite = _itemBoxType.Sprite;
        _onDestroy = killAction;
    }

    private void OnMouseDown()
    {
        SpawnRandomItem();

        _onDestroy(this);
    }

    private void SpawnRandomItem()
    {
        var randomValue = UnityEngine.Random.Range(0f, 1f);
        var possibleDrops = _itemBoxType.Drops.OrderBy(drop => drop.DropChance).ToArray();

        List<float> cumulativeDropChances = new List<float>();
        float cumulativeChance = 0f;

        for (int i = 0; i < possibleDrops.Length; i++)
        {
            cumulativeChance += possibleDrops[i].DropChance;
            cumulativeDropChances.Add(cumulativeChance);
        }

        for (int i = 0; i < cumulativeDropChances.Count; i++)
        {
            if (randomValue <= cumulativeDropChances[i])
            {
                _mergeGrid.CreateItem(possibleDrops[i].Type, (Cell)_cell);
                return;
            }
        }
    }
}
