using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : Singleton<EnemySpawner>, IUpdater
{
    [SerializeField] private Transform _playerTransform;

    // ----------------------------------------------------------------
    [Serializable]
    public class Wave
    {
        public string WaveName;
        public List<EnemyGroup> EnemyGroups;
        [HideInInspector] public int WaveQuota;
        public float SpawnInterval;
        [HideInInspector] public int SpawnCount;
    }

    [Serializable]
    public class EnemyGroup
    {
        public string EnemyName;
        public int EnemyCount;
        [HideInInspector] public int SpawnCount;
        public GameObject EnemyPrefab;
    }

    private enum SpawnerState
    {
        Waiting,
        Spawning,
        WaitingForClear,
        Finished
    }

    // ----------------------------------------------------------------
    private SpawnerState _currentState = SpawnerState.Waiting;
    
    [Header("Wave Settings")]
    public List<Wave> Waves;
    private int _currentWaveCount;

    [Header("Spawner Attributes")]
    [SerializeField] private int _maxEnemiesAllowed = 10;
    private float _spawnTimer;
    private int _enemiesAlive;

    [SerializeField] private float _waveInterval = 5f;

    [Header("Spawn Positions")]
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Collider2D _validSpawnArea;
    
    private List<Transform> _availSpawnPoints;

    // ===========================================================================
    private void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);

        this.Subscribe<OnEnemyKilledEvent>(OnEnemyKilled);
    }

    private void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnUnassignUpdater(this);
        }

        if (EventDispatcher.Instance != null)
        {
            this.Unsubscribe<OnEnemyKilledEvent>(OnEnemyKilled);
        }
    }

    void Start()
    {
        _availSpawnPoints = new List<Transform>(_spawnPoints);
        CalculateWaveQuota();

        StartCoroutine(BeginNextWave());
    }

    
    public void OnUpdate()
    {
        if (_currentState == SpawnerState.Spawning)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= Waves[_currentWaveCount].SpawnInterval)
            {
                _spawnTimer = 0f;
                SpawnSingleEnemy();
            }

            if (Waves[_currentWaveCount].SpawnCount >= Waves[_currentWaveCount].WaveQuota)
            {
                _currentState = SpawnerState.WaitingForClear;
            }
        }
        else if (_currentState == SpawnerState.WaitingForClear)
        {
            if (_enemiesAlive <= 0)
            {
                if (_currentWaveCount < Waves.Count - 1)
                {
                    _currentWaveCount++;
                    CalculateWaveQuota();
                    StartCoroutine(BeginNextWave());
                }
                else
                {
                    _currentState = SpawnerState.Finished;
                }
            }
        }
    }

    
    private IEnumerator BeginNextWave()
    {
        _currentState = SpawnerState.Waiting;

        yield return new WaitForSeconds(_waveInterval);

        _currentState = SpawnerState.Spawning;
        _spawnTimer = Waves[_currentWaveCount].SpawnInterval;
    }

    
    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in Waves[_currentWaveCount].EnemyGroups)
        {
            currentWaveQuota += enemyGroup.EnemyCount;
        }

        Waves[_currentWaveCount].WaveQuota = currentWaveQuota;
    }

    
    public void UpdateAvailableSpawnPoints()
    {
        _availSpawnPoints.Clear();
        foreach (Transform point in _spawnPoints)
        {
            if (_validSpawnArea != null && _validSpawnArea.OverlapPoint(point.position))
            {
                _availSpawnPoints.Add(point);
            }
        }
    }

    
    private void SpawnSingleEnemy()
    {
        if (_enemiesAlive >= _maxEnemiesAllowed) return;


        Wave currentWave = Waves[_currentWaveCount];

        List<EnemyGroup> validGroups = new List<EnemyGroup>();
        foreach (var enemyGroup in currentWave.EnemyGroups)
        {
            if (enemyGroup.SpawnCount < enemyGroup.EnemyCount)
            {
                validGroups.Add(enemyGroup);
            }
        }

        if (validGroups.Count == 0) return;

        EnemyGroup groupToSpawn = validGroups[Random.Range(0, validGroups.Count)];

        UpdateAvailableSpawnPoints();
        if (_availSpawnPoints.Count == 0) return;

        Vector3 spawnPos = GetRandomAvailableSpawnPoint();
        GameObject spawnedEnemy = PoolManager.Instance.Spawn(groupToSpawn.EnemyPrefab, spawnPos, Quaternion.identity);

        if (spawnedEnemy.TryGetComponent(out MobController mob))
        {
            mob.Init(_playerTransform, spawnPos);
        }
        else if (spawnedEnemy.TryGetComponent(out MobControllerSP mobSP))
        {
            mobSP.Init(_playerTransform, spawnPos);
        }

        groupToSpawn.SpawnCount++;
        currentWave.SpawnCount++;
        _enemiesAlive++;
    }

    
    private void OnEnemyKilled(OnEnemyKilledEvent eventData)
    {
        _enemiesAlive--;
    }

    
    public Vector3 GetRandomAvailableSpawnPoint()
    {
        int index = Random.Range(0, _availSpawnPoints.Count);
        return _availSpawnPoints[index].position;
    }
}