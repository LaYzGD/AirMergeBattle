using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ItemBox", fileName = "New ItemBox")]
public class ItemBoxType : ScriptableObject
{
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public DropInfo[] Drops { get; private set; }
}

[Serializable]
public struct DropInfo 
{
    [field:SerializeField] public TurretType Type { get; private set; }
    [field: SerializeField] public float DropChance { get; private set; }
}
