using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
[RequireComponent(typeof (Collider2D))]
[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int _damage;
    private float _movementSpeed;
    private float _lifeTime;
    private float _explosionRadius;
    private bool _isAutoAiming;

    private LayerMask _enemyLayer;
    private Base _playerBase;

    private Transform _target;

    private Action<Projectile> _destroyAction;

    [Inject]
    public void Construct(Base playerBase)
    {
        _playerBase = playerBase;
    }

    public void Initialize(ProjectileData data, int damage, LayerMask enemyLayer)
    {
        _spriteRenderer.sprite = data.Sprite;
        _damage = damage;
        _movementSpeed = data.MovementSpeed;
        _lifeTime = data.LifeTime;
        _explosionRadius = data.ExplosionRadius;
        _enemyLayer = enemyLayer;
        _isAutoAiming = data.IsAutoAiming;
    }

    public void SetKillAction(Action<Projectile> killAction)
    {
        _destroyAction = killAction;
    }

    public void StartMovement()
    {
        Invoke(nameof(Destroy), _lifeTime);
        
        if (_isAutoAiming)
        {
            _target = _playerBase.GetClosestEnemy();
            if (_target != null)
            {
                StartCoroutine(FollowTarget());
                return;
            }
        }

        _rigidBody2D.linearVelocityY = _movementSpeed;
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

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            _rigidBody2D.position = Vector2.MoveTowards(_rigidBody2D.position, _target.position, _movementSpeed * Time.fixedDeltaTime);
            yield return null;
        }
    }

    private void Destroy()
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

        StopAllCoroutines();
        _destroyAction(this);
    }
}
