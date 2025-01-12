using System;
using UnityEngine;
using Zenject;

public class Base : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;

    [SerializeField] private Vector2 _screenCenter = Vector2.zero;
    [SerializeField] private float _enemyDetectionRadius;
    [SerializeField] private LayerMask _enemyLayer;

    private GlobalStats _globalStats;
    private float _currentHealth;
    private float _currentMaxHealth;

    public event Action OnBaseDestroyed;
    public event Action<float, float> OnHealthUpdate;

    public float MaxHealth => _currentMaxHealth;
    public float CurrentHealth => _currentHealth;

    [Inject]
    public void Construct(GlobalStats stats) 
    {
        _globalStats = stats;
        _currentMaxHealth = _maxHealth + _globalStats.GetStat(StatType.BaseHealth).CurrentValue;
        _currentHealth = _currentMaxHealth;
    }

    public void IncreaseMaxHealth()
    {
        _currentMaxHealth = _maxHealth + _globalStats.GetStat(StatType.BaseHealth).CurrentValue;
        OnHealthUpdate?.Invoke(_currentHealth, _currentMaxHealth);
    }

    public void Restart()
    {
        _currentHealth = _currentMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        _currentHealth -= damage;
        OnHealthUpdate?.Invoke(_currentHealth, _currentMaxHealth);
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Destroy();
        }
    }

    public Transform GetClosestEnemy()
    {
        var enemies = Physics2D.OverlapCircleAll(_screenCenter, _enemyDetectionRadius, _enemyLayer);

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            Vector2 diractionToTarget = transform.position - enemy.transform.position;
            float distanceSqrToTarget = diractionToTarget.sqrMagnitude;
            
            if (distanceSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToTarget;
                bestTarget = enemy.transform;
            }
        }

        return bestTarget;
    }

    private void Destroy()
    {
        OnBaseDestroyed?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_screenCenter, _enemyDetectionRadius);
    }
}
