using UnityEngine;

[CreateAssetMenu(menuName = "Data/Projectile", fileName = "New ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float LifeTime { get; private set; }
    [field: SerializeField] public bool IsAutoAiming { get; private set; } = false;
    [field: SerializeField] public float ExplosionRadius { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}
