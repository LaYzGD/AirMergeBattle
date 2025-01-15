using UnityEngine;

[CreateAssetMenu(menuName = "Data/Turrets/TurretType", fileName = "New TurretType")]
[System.Serializable]
public class TurretType : ScriptableObject
{
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public TurretType NextUpgrade { get; private set; }
    [field: SerializeField] public ProjectileData ProjectileData { get; private set; }
    [field: SerializeField] public float DistanceBetweenProjectiles { get; private set; } = 0.2f;
    [field: SerializeField] public float ShootingDelay { get; private set; } = 0.5f;
    [field: SerializeField] public int ProjectilesAmount { get; private set; } = 1;
    [field: SerializeField] public int Damage { get; private set; } = 1;
}
