﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemShoot : MonoBehaviour
{
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private float _shootingSoundVolume;

    private ProjectilePool _projectilePool;
    private ProjectileData _projectileData;

    private float _damage;
    private float _shootingDelay;
    private int _projectileAmount;
    private float _distanceBetweenProjectiles;

    private Vector2[] _projectilePositions;

    private Coroutine _coroutine;
    private GlobalStats _globalStats;
    private AudioPlayer _audioPlayer;
    private WaveSpawner _spawner;
    private TurretType _turretType;

    private bool _isPaused;

    [Inject]
    public void Construct(ProjectilePool pool, GlobalStats stats, AudioPlayer audioPlayer, WaveSpawner spawner)
    {
        _projectilePool = pool;
        _globalStats = stats;
        _audioPlayer = audioPlayer;
        _spawner = spawner;
        _spawner.OnWaveStarted += ContinueShooting;
        _spawner.OnWaveFinished += PauseShooting;
    }

    public void Init(TurretType type)
    {
        _projectileData = type.ProjectileData;
        _turretType = type;
        _damage = type.Damage;
        _shootingDelay = type.ShootingDelay;
        _projectileAmount = type.ProjectilesAmount;
        _distanceBetweenProjectiles = type.DistanceBetweenProjectiles;
    }

    public void StartShooting()
    {
        if (_isPaused)
        {
            return;
        }

        _projectilePositions = CalculateProjectilePositions(_projectileAmount);
        _coroutine = StartCoroutine(Shoot());
    }

    private void ContinueShooting()
    {
        if (_coroutine == null)
        {
            return;
        }

        _isPaused = false;
        StartShooting();
    }

    private void PauseShooting()
    {
        if (_coroutine == null)
        {
            return;
        }

        _isPaused = true;
        StopCoroutine(_coroutine);
    }

    public void StopShooting()
    {
        if (_coroutine == null)
        {
            return;
        }

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator Shoot()
    {
        while (true) 
        {
            var shootingDelay = _shootingDelay - (_turretType.ShootingDelay * _globalStats.GetStat(StatType.StructureDelay).CurrentValue);
            var damage = _damage + (_turretType.Damage * _globalStats.GetStat(StatType.StructureDamage).CurrentValue);

            yield return new WaitForSecondsRealtime(shootingDelay);

            _audioPlayer.PlaySound(_shootingSound, _shootingSoundVolume);

            for (int i = 0; i < _projectileAmount; i++)
            {
                var projectile = _projectilePool.GetProjectile();
                projectile.transform.position = _projectilePositions[i];
                projectile.Initialize(_projectileData, damage, _enemyLayer);
                projectile.StartMovement();
            }
        }
    }

    private Vector2[] CalculateProjectilePositions(int projectileAmount)
    {
        if (projectileAmount == 1)
            return new Vector2[] { _shootingPoint.position };

        List<Vector2> positions = new List<Vector2>();
        int halfRange = projectileAmount / 2;

        for (int i = 0; i < projectileAmount; i++)
        {
            var param = (-halfRange + i);

            if (projectileAmount % 2 == 0)
            {
                param = (i < halfRange ? -halfRange + i : 1 + (i - halfRange));
            }

            positions.Add(new Vector2(_shootingPoint.position.x + param * _distanceBetweenProjectiles, _shootingPoint.position.y));
        }

        return positions.ToArray();
    }
}
