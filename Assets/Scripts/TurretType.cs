using UnityEngine;

[CreateAssetMenu(menuName = "Data/Turrets/TurretType", fileName = "New TurretType")]
public class TurretType : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public TurretType NextUpgrade { get; private set; }
    //[field: SerializeField] public Projectile Projectile { get; private set; }
    [field: SerializeField] public float ShootingSpeed { get; private set; }
}
