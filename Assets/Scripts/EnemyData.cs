using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy", fileName = "New Enemy")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int Healh { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } = 20;
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public VFXObjectData VFXData { get; private set; }
}
