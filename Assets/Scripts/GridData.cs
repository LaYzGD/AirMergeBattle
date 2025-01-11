using UnityEngine;

[CreateAssetMenu(menuName = "Data/Grid", fileName = "New GridData")]
public class GridData : ScriptableObject
{
    [field: SerializeField] public int Height { get; private set; } = 5;
    [field: SerializeField] public int Width { get; private set; } = 5;
    [field: SerializeField] public float CellSize { get; private set; } = 1f;
}
