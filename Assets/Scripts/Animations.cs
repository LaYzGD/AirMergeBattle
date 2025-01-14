using UnityEngine;
using DG.Tweening;

public class Animations : MonoBehaviour
{
    public void PlayPunchAnimation(AnimationData data, Transform transform, Vector3 scale)
    {
        transform.DOPunchScale(scale * data.PunchScale, data.AnimationDuration, data.Vibrato, data.Elastisity).SetEase(Ease.InQuad).SetAutoKill()/*.OnKill(() => transform.localScale = initialScale)*/;
    }
}
