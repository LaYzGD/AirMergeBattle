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

    private float _currentHealth;
    private Action<Enemy> _killAction;

    private bool _isLast;

    private Money _money;
    private WaveSpawner _waveSpawner;

    [Inject]
    public void Construct(Money money, WaveSpawner spawner)
    {
        _money = money;
        _waveSpawner = spawner;
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

    public void TakeDamage(float damage) 
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
            SendMessages();
            _killAction(this);
        }
    }

    private void SendMessages()
    {
        if (_isLast)
        {
            _waveSpawner.FinishWave();
            return;
        }

        _waveSpawner.UpdateWaveProgress();
    }

    private void Destroy()
    {
        SendMessages();
        _money.AddMoney(_enemyData.Cost);
        _killAction(this);
    }
}
