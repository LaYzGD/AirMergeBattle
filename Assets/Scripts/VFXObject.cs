using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFXObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private Action<VFXObjectType, VFXObject> _action;
    public VFXObjectType Type { get; private set; }

    public void Init(VFXObjectType type, Action<VFXObjectType, VFXObject> action)
    {
        _action = action;
    }

    public void Play()
    {
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        _action(Type, this);
    }
}

[Serializable]
public enum VFXObjectType 
{
    ProjectileExplosion,
    BigExplosion,
    PlaneExplosion,
    Merge
}
