using UnityEngine;

[CreateAssetMenu(menuName = "Data/Animation", fileName = "New AnimationData")]
public class AnimationData : ScriptableObject
{
    [field: SerializeField] public float PunchScale { get; private set; } = 0.5f;
    [field: SerializeField] public float AnimationDuration { get; private set; } = 0.2f;
    [field: SerializeField] public int Vibrato { get; private set; } = 1;
    [field: SerializeField] public float Elastisity { get; private set; } = 0.4f;
}
