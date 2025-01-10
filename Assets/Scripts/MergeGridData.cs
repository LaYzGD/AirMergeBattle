using UnityEngine;

[CreateAssetMenu(menuName = "Data/Grid/MergeGrid", fileName = "New MergeGridData")]
public class MergeGridData : ScriptableObject
{
    [field: SerializeField] public int Height { get; private set; } = 5;
    [field: SerializeField] public int Width { get; private set; } = 5;
    [field: SerializeField] public float CellSize { get; private set; } = 1f;
    [field: SerializeField] public Color CellWhiteColor { get; private set; }
    [field: SerializeField] public Color CellBlackColor { get; private set; }
}
