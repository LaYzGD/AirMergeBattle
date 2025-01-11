using UnityEngine;

public class Base : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;

    [SerializeField] private Vector2 _screenCenter = Vector2.zero;
    [SerializeField] private float _enemyDetectionRadius;
    [SerializeField] private LayerMask _enemyLayer;

    private int _currentHealth;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

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

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_screenCenter, _enemyDetectionRadius);
    }
}
