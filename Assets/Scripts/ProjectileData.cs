using UnityEngine;

[CreateAssetMenu(menuName = "Data/Projectile", fileName = "New ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; } = 200f;
    [field: SerializeField] public float LifeTime { get; private set; }
    [field: SerializeField] public bool IsAutoAiming { get; private set; } = false;
    [field: SerializeField] public float ExplosionRadius { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public AudioClip ExplosionSound { get; private set; }
    [field: SerializeField] public float ExplosionSoundVolume { get; private set; } = 0.5f;
    [field: SerializeField] public VFXObjectData VFXData { get; private set; }
}
