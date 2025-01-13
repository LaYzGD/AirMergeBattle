using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private WaveData[] _waves;
    [SerializeField] private Transform _enemiesSpawnPoint;
    [SerializeField] private float _yPositionThreshold = 5f;

    private ObjectPool<Enemy> _enemyPool;

    private WaveData _currentWave;
    private int _currentWaveIndex = 0;
    private int _currentWaveNumber = 1;

    private Dictionary<EnemyData, int> _currentWaveEnemies;
    private int _allCurrentWaveEnemiesAmount;
    private int _destroyedEnemies;

    private bool _waveIsConfigured;
    private List<Enemy> _activeEnemies;

    public int CurrentReward { get; private set; }

    public int CurrentWaveNumber => _currentWaveNumber;

    public event Action<int> OnWaveFinished;
    public event Action<int> OnWaveProgressUpdate;
    public event Action<int> OnWaveConfigured;


    private void Start()
    {
        _enemyPool = new ObjectPool<Enemy>(OnCreate, OnGet, OnRelease, OnEnemyDestroy, false);

        SpawnWave();
    }

    private void ConfigureWave()
    {
        _currentWave = _waves[_currentWaveIndex];
        _currentWaveEnemies = new Dictionary<EnemyData, int>();
        _activeEnemies = new List<Enemy>();
        _allCurrentWaveEnemiesAmount = 0;

        for (int i = 0; i < _currentWave.AllEnemies.Length; i++)
        {
            var info = _currentWave.AllEnemies[i];
            _allCurrentWaveEnemiesAmount += info.Amount;
            _currentWaveEnemies.Add(info.EnemyType, info.Amount);
        }

        _destroyedEnemies = 0;
        CurrentReward = _currentWave.Reward;

        _waveIsConfigured = true;
        OnWaveConfigured?.Invoke(_allCurrentWaveEnemiesAmount);
    }

    private void SpawnEnemy(Vector2 position, EnemyData data, bool isLast = false)
    {
        var enemy = _enemyPool.Get();
        enemy.Init(data, KillAction, isLast);
        enemy.transform.SetParent(transform);
        enemy.transform.position = position;
        enemy.StartMovement();
        _activeEnemies.Add(enemy);
    }

    public void SpawnWave()
    {
        if (!_waveIsConfigured)
        {
            ConfigureWave();
        }

        StartCoroutine(SpawnWaveCoroutine());
    }

    public void RestartCurrentWave()
    {
        Stop();

        SpawnWave();
    }

    private void RemoveActiveEnemies()
    {
        if (_activeEnemies.Count > 0)
        {
            foreach (var enemy in _activeEnemies)
            {
                _enemyPool.Release(enemy);
            }
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        _waveIsConfigured = false;

        RemoveActiveEnemies();
    }

    public void FinishWave()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex >= _waves.Length)
        {
            _currentWaveIndex = _waves.Length - 1;
        }
        _currentWaveNumber++;
        _waveIsConfigured = false;
        OnWaveFinished?.Invoke(_currentWaveNumber);
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        for (int i = 0; i < _allCurrentWaveEnemiesAmount; i++)
        {
            var enemyTypes = _currentWaveEnemies.Keys.ToArray();
            var randomEnemy = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];
            if (_currentWaveEnemies.TryGetValue(randomEnemy, out int value))
            {
                SpawnEnemy(new Vector2(_enemiesSpawnPoint.position.x, _enemiesSpawnPoint.position.y + (UnityEngine.Random.Range(-1f, 1f) * _yPositionThreshold)), randomEnemy, i == _allCurrentWaveEnemiesAmount - 1);
                value--;
                if (value == 0)
                {
                    _currentWaveEnemies.Remove(randomEnemy);
                }
            }
            yield return new WaitForSecondsRealtime(_currentWave.DelayBetweenSpawn);
        }
    }

    private void KillAction(Enemy enemy)
    {
        _enemyPool.Release(enemy);

        _activeEnemies.Remove(enemy);
        _destroyedEnemies++;
        OnWaveProgressUpdate?.Invoke(_destroyedEnemies);

        if (enemy.IsLast)
        {
            FinishWave();
        }
    }

    private Enemy OnCreate()
    {
        return Instantiate(_enemyPrefab);
    }

    private void OnGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnEnemyDestroy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
