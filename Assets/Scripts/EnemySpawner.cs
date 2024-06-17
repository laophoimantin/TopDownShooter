using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // A list of groups of enemies to spawn in this wave
        public int waveQuota; // The total number of enemies to spawn in this wave
        public float spawnInterval; // The interval at which to spawn enemies
        public int spawnCount; // The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // The number of enemies to spawn in this wave
        public int spawnCount; // The number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // A list of all the wave in the game
    public int currentWaveCount; //The index of the current wave [a list starts from 0]

    [Header("Spawner Attributes")]
    private float spawnTimer;
    [SerializeField] private int enemiesAlive;
    [SerializeField] private int maxEnemiesAllowed;
    private bool maxEnemiesReached;
    private bool isWaveActive = false;

    [SerializeField] private float waveInterval; // The interval between each wave

    [Header("Spawn Posistions")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Collider2D validSpawnArea;

    [Header("Valid Spawn Positions")]
    public List<Transform> availSpawnPoints;

    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CalculateWaveQuota();
        availSpawnPoints = new List<Transform>(spawnPoints);
        firstWave();
    }

    void firstWave()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer = 0f;
        SpawnEnemies();
    }

    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive) // Check if the wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;
        
        // Check if is's time to spawn the next enemy
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }

    }

    private IEnumerator BeginNextWave()
    {
        isWaveActive = true;

        // Wave for "waveInterval seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        // If there are more waves to start after the current wave, move on to the next wave
        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach ( var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }

    public void UpdateAvailableSpawnPoints()
    {
        availSpawnPoints.Clear();
        foreach (Transform point in spawnPoints)
        {
            if (validSpawnArea.OverlapPoint(point.position))
            {
                availSpawnPoints.Add(point);
            }
        }
    }

    private void SpawnEnemies()
    {
        // Check if the minimum number of enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            // Spawn each type of enemy until the quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // Check if the minimum number of enemies of this type have been spawned 
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    UpdateAvailableSpawnPoints();
                    int randomIndex = Random.Range(0, availSpawnPoints.Count);
                    Vector2 finalSpawnPoint = availSpawnPoints[randomIndex].position;
                    Instantiate(enemyGroup.enemyPrefab, finalSpawnPoint, Quaternion.identity);
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    // Limit the number of enemies that can be spawned at one
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }

        }

    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        //Reset the maxEnemiesReached flag if the number of enemies alive has dropped below the maximum amount
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}
