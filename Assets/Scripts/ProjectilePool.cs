using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;

    private ObjectPool<Projectile> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease, OnProjectileDestroy, false);
    }

    public Projectile GetProjectile()
    {
        var projectile = _pool.Get();
        projectile.SetKillAction(KillAction);
        projectile.transform.SetParent(transform);
        return projectile;
    }

    private void KillAction(Projectile projectile)
    {
        _pool.Release(projectile);
    }

    private Projectile OnCreate()
    {
        return Instantiate(_projectilePrefab);
    }

    private void OnGet(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnRelease(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnProjectileDestroy(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
