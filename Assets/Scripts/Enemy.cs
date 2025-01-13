using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _destroySound;
    [SerializeField] private float _destroySoundVolume;

    private EnemyData _enemyData;

    private float _currentHealth;
    private Action<Enemy> _killAction;
    public bool IsLast { get; private set; }

    private VFXObjectData _vfxData;
    private VFXPool _vfxPool;
    private Money _money;
    private AudioPlayer _audioPlayer;

    [Inject]
    public void Construct(Money money, AudioPlayer audioPlayer, VFXPool vfxPool)
    {
        _money = money;
        _audioPlayer = audioPlayer;
        _vfxPool = vfxPool;
    }

    public void Init(EnemyData data, Action<Enemy> killAction, bool isLast = false)
    {
        _enemyData = data;
        _currentHealth = _enemyData.Healh;
        _vfxData = _enemyData.VFXData;
        _killAction = killAction;
        _spriteRenderer.sprite = _enemyData.Sprite;
        IsLast = isLast;
    }

    public void StartMovement()
    {
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.linearVelocityX = _enemyData.MovementSpeed;
    }

    public void TakeDamage(float damage) 
    {
        if (damage <= 0 || _currentHealth <= 0)
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
            _killAction(this);
        }
    }


    private void Destroy()
    {
        _money.AddMoney(_enemyData.Cost);
        _vfxPool.SpawnVFX(_vfxData, transform.position);
        _audioPlayer.PlaySound(_destroySound, _destroySoundVolume);
        _killAction(this);
    }
}
