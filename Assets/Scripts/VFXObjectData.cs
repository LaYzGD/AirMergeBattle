using UnityEngine;

[CreateAssetMenu(menuName = "Data/VFX", fileName = "New VFXObjectData")]
public class VFXObjectData : ScriptableObject
{
    [field: SerializeField] public VFXObjectType VFXType { get; private set; }
    [field: SerializeField] public VFXObject Prefab { get; private set; }
}