using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFXPool : MonoBehaviour
{
    private Dictionary<VFXObjectType, ObjectPool<VFXObject>> _pools = new ();

    public void CreatePool(VFXObjectType type, VFXObject prefab)
    {
        _pools.Add(type, new ObjectPool<VFXObject>(() => Instantiate(prefab), (vfx) => vfx.gameObject.SetActive(true), (vfx) => vfx.gameObject.SetActive(false), (vfx) => Destroy(vfx.gameObject), false));
    }

    public void SpawnVFX(VFXObjectType type, Vector2 position, VFXObject prefab)
    {
        if (!_pools.ContainsKey(type)) 
        {
            CreatePool(type, prefab);
        }

        var pool = _pools[type];
        var vfx = pool.Get();
        vfx.Init(type, OnVFXStop);
        vfx.transform.position = position;
        vfx.Play();
    }

    private void OnVFXStop(VFXObjectType type, VFXObject vfx)
    {
        if (!_pools.ContainsKey(type))
        {
            Destroy(vfx.gameObject);
            return;
        }

        var pool = _pools[type];
        pool.Release(vfx);
    }
}
