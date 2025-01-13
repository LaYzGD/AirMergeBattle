using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private VFXObjectData _vfxData;

    private float _damage;
    private float _movementSpeed;
    private float _lifeTime;
    private float _explosionRadius;
    private bool _isAutoAiming;
    private float _rotationSpeed;

    private LayerMask _enemyLayer;
    private Base _playerBase;
    private GlobalStats _globalStats;
    private AudioPlayer _audioPlayer;
    private AudioClip _explosionSound;
    private float _explosionSoundVolume;

    private VFXPool _vfxPool;
    private Transform _target;

    private Action<Projectile> _destroyAction;

    [Inject]
    public void Construct(Base playerBase, GlobalStats stats, AudioPlayer audioPlayer, VFXPool vfxPool)
    {
        _playerBase = playerBase;
        _globalStats = stats;
        _audioPlayer = audioPlayer;
        _vfxPool = vfxPool;
    }

    public void Initialize(ProjectileData data, float damage, LayerMask enemyLayer)
    {
        _spriteRenderer.sprite = data.Sprite;
        _damage = damage;
        _movementSpeed = data.MovementSpeed + (data.MovementSpeed * _globalStats.GetStat(StatType.ProjectileSpeed).CurrentValue);
        _lifeTime = data.LifeTime;
        _explosionRadius = data.ExplosionRadius;
        _enemyLayer = enemyLayer;
        _isAutoAiming = data.IsAutoAiming;
        _vfxData = data.VFXData;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _rotationSpeed = data.RotationSpeed + (data.RotationSpeed * _globalStats.GetStat(StatType.ProjectileSpeed).CurrentValue);
        _explosionSound = data.ExplosionSound;
        _explosionSoundVolume = data.ExplosionSoundVolume;
    }

    public void SetKillAction(Action<Projectile> killAction)
    {
        _destroyAction = killAction;
    }

    public void StartMovement()
    {
        StartCoroutine(CheckLifeTime());

        if (_isAutoAiming)
        {
            FindTarget(true);
        }

        _rigidBody2D.linearVelocityY = _movementSpeed;
    }

    private void FindTarget(bool simpleKill)
    {
        _target = _playerBase.GetClosestEnemy();
        if (_target != null)
        {
            StartCoroutine(FollowTarget());
            return;
        }

        if (simpleKill)
        {
            _destroyAction(this);
            return;
        }

        Destroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Mathf.Log(_enemyLayer.value, 2))
        {
            return;
        }

        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
            Destroy();
        }
    }

    private IEnumerator CheckLifeTime()
    {
        while (_lifeTime > 0)
        {
            _lifeTime -= Time.deltaTime;
            yield return null;
        }

        Destroy(false);
    }

    private IEnumerator FollowTarget()
    {
        while (_target != null && _target.gameObject.activeSelf)
        {
            Vector2 direction = (Vector2)_target.position - _rigidBody2D.position;
            direction.Normalize();

            float rotationAmount = Vector3.Cross(direction, transform.up).z;

            _rigidBody2D.angularVelocity = -rotationAmount * _rotationSpeed;
            _rigidBody2D.linearVelocity = transform.up * _movementSpeed;
            yield return null;
        }

        FindTarget(false);
    }

    private void Destroy(bool hasSound = true)
    {
        if (_explosionRadius > 0)
        {
            var enemies = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayer);
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }

        _vfxPool.SpawnVFX(_vfxData.VFXType, transform.position, _vfxData.Prefab);

        if (hasSound)
        {
            _audioPlayer.PlaySound(_explosionSound, _explosionSoundVolume);
        }

        StopAllCoroutines();
        _destroyAction(this);
    }
}
