using System.Linq;
using UnityEngine;

public class AllTurretUpgrades : MonoBehaviour
{
    [SerializeField] private TurretType[] _turretTypes;

    public TurretType GetTurretByIndex(int index)
    {
        return _turretTypes.FirstOrDefault(t => t.Index == index);
    }
}
