using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private EnemyData _enemyData;

    private int _currentHealth;
    private Action<Enemy> _killAction;

    private bool _isLast;

    private Money _money;

    [Inject]
    public void Construct(Money money)
    {
        _money = money;
    }

    public void Init(EnemyData data, Action<Enemy> killAction, bool isLast = false)
    {
        _enemyData = data;
        _currentHealth = _enemyData.Healh;
        _killAction = killAction;
        _isLast = isLast;
        _spriteRenderer.sprite = _enemyData.Sprite;
    }

    public void StartMovement()
    {
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.linearVelocityX = _enemyData.MovementSpeed;
    }

    public void TakeDamage(int damage) 
    {
        if (damage <= 0)
        {
            return;
        }

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Destroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Base playerBase))
        {
            playerBase.TakeDamage(_enemyData.Damage);
            if (_isLast)
            {
                print("Wave Finished");
            }
            _killAction(this);
        }
    }

    private void Destroy()
    {
        if (_isLast)
        {
            print("Wave Finished");
        }
        _money.AddMoney(_enemyData.Cost);
        _killAction(this);
    }
}
