using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool gunDemoMode;

    public static GameController instance;
    public Transform player;
    CharacterController playerScript;
    public CharacterController PlayerScript { get { return playerScript; } }

    // Spawning enemies
    public GameObject enemyPF;
    public Transform enemyHolder;
    
    float enemySpawnTimer = 0f;
    float enemySpawnTimerCD = 0f;
    int maxEnemyCount = 10000;

    // Enemy logic
    Dictionary<int, List<Enemy>> enemyBatches = new Dictionary<int, List<Enemy>>();
    float runLogicTimer = 0f;
    float runLogicTimerCD = 1f;

    //* SPATIAL PARTITIONING *//
    int spatialGroupWidth = 100;
    public int SpatialGroupWidth { get { return spatialGroupWidth; } }

    int spatialGroupHeight = 100;
    public int SpatialGroupHeight { get { return spatialGroupHeight; } }

    int numberOfPartitions = 10000;
    public int NumberOfPartitions { get { return numberOfPartitions; } }

    int mapWidthMin = - 1;
    public int MAP_WIDTH_MIN { get { return mapWidthMin; } }
    int mapWidthMax = -1;
    public int MAP_WIDTH_MAX { get { return mapWidthMax; } }
    int mapHeightMin = -1;
    public int MAP_HEIGHT_MIN { get { return mapHeightMin; } }
    int mapHeightMax = -1;
    public int MAP_HEIGHT_MAX { get { return mapHeightMax; } }

    // For enemies
    [HideInInspector] public Dictionary<int, HashSet<Enemy>> enemySpatialGroups = new Dictionary<int, HashSet<Enemy>>();

    // For bullets
    [HideInInspector] public Dictionary<int, HashSet<Bullet>> bulletSpatialGroups = new Dictionary<int, HashSet<Bullet>>();

    // For experience points
    public GameObject experiencePointPF;
    public Transform experiencePointHolder;

    // For get spatial group STATIC (more efficient) calculations
    int CELLS_PER_ROW_STATIC;
    int CELLS_PER_COLUMN_STATIC; // Square grid assumption
    float CELL_WIDTH_STATIC;
    float CELL_HEIGHT_STATIC;
    int HALF_WIDTH_STATIC;
    int HALF_HEIGHT_STATIC;

    //! MIN HEAP FOR BATCH //
    public class BatchScore : System.IComparable<BatchScore>
    {
        public int BatchId { get; }
        public int Score { get; private set; }

        public BatchScore(int batchId, int score)
        {
            BatchId = batchId;
            Score = score;
        }

        public void UpdateScore(int delta)
        {
            Score += delta;
        }

        public int CompareTo(BatchScore other)
        {
            int scoreComparison = Score.CompareTo(other.Score);
            if (scoreComparison == 0)
            {
                // When scores are equal, further compare based on BatchId
                return BatchId.CompareTo(other.BatchId);
            }
            return scoreComparison;
        }
    }

    SortedSet<BatchScore> batchQueue_Enemy = new SortedSet<BatchScore>();

    // Keeps track of the current score of each batch
    Dictionary<int, BatchScore> batchScoreMap_Enemy = new Dictionary<int, BatchScore>();

    public void AddToEnemyBatch(int batchId, Enemy enemy) { enemyBatches[batchId].Add(enemy); }

    public void UpdateBatchOnUnitDeath(string option, int batchId)
    {
        if (option == "enemy") UpdateBatchOnEnemyDeathRaw(batchQueue_Enemy, batchScoreMap_Enemy, batchId);
    }

    void UpdateBatchOnEnemyDeathRaw(SortedSet<BatchScore> batchQueue, Dictionary<int, BatchScore> batchScoreMap, int batchId)
    {
        if (batchScoreMap.ContainsKey(batchId))
        {
            BatchScore batchScore = batchScoreMap[batchId];

            batchQueue.Remove(batchScore); // Remove OLD

            batchScore.UpdateScore(-1);
            
            batchQueue.Add(batchScore); // Add NEW
        }
        else
        {
            Debug.Log("THIS SHOULDN'T HAPPEN");
        }
    }

    public int GetBestBatch(string option)
    {
        if (option == "enemy") return GetBestBatchRaw(batchQueue_Enemy);
        else return -1;
    }

    int GetBestBatchRaw(SortedSet<BatchScore> batchQueue)
    {
        BatchScore leastLoadedBatch = batchQueue.Min;

        // Debug.Log("Least loaded: " + leastLoadedBatch.BatchId + ", score: " + leastLoadedBatch.Score);

        if (leastLoadedBatch == null)
        {
            // Handle the case where there are no batches
            Debug.Log("THIS SHOULDN'T HAPPEN");
            return 0;
        }

        batchQueue.Remove(leastLoadedBatch); // Remove OLD

        leastLoadedBatch.UpdateScore(1);

        batchQueue.Add(leastLoadedBatch); // Add NEW

        return leastLoadedBatch.BatchId;
    }

    void InitializeBatches()
    {
        for (int i = 0; i < 50; i++)
        {
            BatchScore batchScore = new BatchScore(i, 0);
            
            // Enemies
            enemyBatches.Add(i, new List<Enemy>()); // batches
            batchScoreMap_Enemy.Add(i, batchScore); // batch scores
            batchQueue_Enemy.Add(batchScore); // batch queue
        }
    }

    //!

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // STATIC GET SPATIAL GROUP ONCE CALCULATIONS
        CELLS_PER_ROW_STATIC = (int)Mathf.Sqrt(numberOfPartitions);
        CELLS_PER_COLUMN_STATIC = CELLS_PER_ROW_STATIC; // Square grid assumption
        CELL_WIDTH_STATIC = spatialGroupWidth / CELLS_PER_ROW_STATIC;
        CELL_HEIGHT_STATIC = spatialGroupHeight / CELLS_PER_COLUMN_STATIC;
        HALF_WIDTH_STATIC = spatialGroupWidth / 2;
        HALF_HEIGHT_STATIC = spatialGroupHeight / 2;

        playerScript = player.GetComponent<CharacterController>();

        InitializeBatches(); //! Initiate batch stuff

        // Create 400 -> 10000 spatial groups
        for (int i = 0; i < numberOfPartitions; i++)
        {
            enemySpatialGroups.Add(i, new HashSet<Enemy>());
            bulletSpatialGroups.Add(i, new HashSet<Bullet>());
        }

        // Spawn 10,000 enemies
        int initEnemyCount = gunDemoMode ? 100 : 10000;
        maxEnemyCount = gunDemoMode ? 100 : 10000;
        for (int i = 0; i < initEnemyCount; i++)
        {
            SpawnEnemy();
        }

        // Set map bounds
        mapWidthMin = -spatialGroupWidth / 2;
        mapWidthMax = spatialGroupWidth / 2;
        mapHeightMin = -spatialGroupHeight / 2;
        mapHeightMax = spatialGroupHeight / 2;
    }

    void FixedUpdate() // 50 frames per second
    {
        if (instance.player == null) return;

        runLogicTimer += Time.deltaTime;

        if (runLogicTimer >= runLogicTimerCD) 
        {
            RunOnceASecondLogicForAllBullets();
            runLogicTimer = 0f;
        }
            
        SpawnEnemies();
        RunBatchLogic((int)(runLogicTimer * 50)); // runLogicTimer is the batchID, for that set of enemies
    }

    void RunOnceASecondLogicForAllBullets()
    {
        // Debug.Log("# of bullets updating: " + bulletSpatialGroups.SelectMany(x => x.Value).Count());
        foreach (Bullet bullet in bulletSpatialGroups.SelectMany(x => x.Value).ToList())
        {
            bullet.OnceASecondLogic();
        }
    }

    void RunBatchLogic(int batchID)
    {
        // Run logic for all enemies in batch
        foreach (Enemy enemy in enemyBatches[batchID])
        {
            if (enemy) enemy.RunLogic();
        }

        // TODO: Clean out previous batch?
    }

    void SpawnEnemies()
    {
        enemySpawnTimer += Time.deltaTime;

        if (enemySpawnTimer > enemySpawnTimerCD && enemyHolder.childCount < maxEnemyCount)
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnEnemy();
            }

            enemySpawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        //! Which batch should it be added?
        int batchToBeAdded = GetBestBatch("enemy");

        // Get the QUADRANT of the player (25 quadrants in the map)
        int playerQuadrant = GetSpatialGroupDynamic(player.position.x, player.position.y, spatialGroupWidth, spatialGroupHeight, 25);
        List<int> expandedSpatialGroups = Utils.GetExpandedSpatialGroups(playerQuadrant, 25);

        // Remove the quadrant player is in
        expandedSpatialGroups.Remove(playerQuadrant);

        // Choose a random spatial group
        int randomSpatialGroup = expandedSpatialGroups[Random.Range(0, expandedSpatialGroups.Count)];

        // Get the center of that spatial group
        Vector2 centerOfSpatialGroup = GetPartitionCenterDynamic(randomSpatialGroup, spatialGroupWidth, spatialGroupHeight, 25);

        // Get a random position within that spatial group
        float sizeOfOneSpatialGroup = spatialGroupWidth / 5; // 100/5 -> 20
        float xVal = Random.Range(centerOfSpatialGroup.x - sizeOfOneSpatialGroup / 2, centerOfSpatialGroup.x + sizeOfOneSpatialGroup / 2);
        float yVal = Random.Range(centerOfSpatialGroup.y - sizeOfOneSpatialGroup / 2, centerOfSpatialGroup.y + sizeOfOneSpatialGroup / 2);

        GameObject enemyGO = Instantiate(enemyPF, enemyHolder);
        enemyGO.transform.position = new Vector3(xVal, yVal, 0);
        enemyGO.transform.parent = enemyHolder;

        Enemy enemyScript = enemyGO.GetComponent<Enemy>();

        // Spatial group
        int spatialGroup = GetSpatialGroup(enemyGO.transform.position.x, enemyGO.transform.position.y);
        enemyScript.spatialGroup = spatialGroup;
        AddToSpatialGroup(spatialGroup, enemyScript);

        // Batch for update logic
        enemyScript.BatchID = batchToBeAdded;
        enemyBatches[batchToBeAdded].Add(enemyScript);
    }

    //? Examples
    // GetSpatialGroup(-50, -50); -->    0
    // GetSpatialGroup( 50, -50); -->   99
    // GetSpatialGroup(-50,  50); --> 9900
    // GetSpatialGroup( 50,  50); --> 9999

    public int GetSpatialGroup(float xPos, float yPos)
    {
        return GetSpatialGroupDynamic(xPos, yPos, spatialGroupWidth, spatialGroupHeight, numberOfPartitions);
    }

    public int GetSpatialGroupStatic(float xPos, float yPos)
    {
        // Adjust positions to map's coordinate system
        float adjustedX = xPos + HALF_WIDTH_STATIC;
        float adjustedY = yPos + HALF_HEIGHT_STATIC;

        // Calculate the indices
        int xIndex = (int)(adjustedX / CELL_WIDTH_STATIC);
        int yIndex = (int)(adjustedY / CELL_HEIGHT_STATIC);

        // Calculate the final index
        return xIndex + yIndex * CELLS_PER_ROW_STATIC;
    }

    int GetSpatialGroupDynamic(float xPos, float yPos, float mapWidth, float mapHeight, int totalPartitions)
    {
        // Calculate the number of cells per row and column, assuming a square grid
        int cellsPerRow = (int)Mathf.Sqrt(totalPartitions);
        int cellsPerColumn = cellsPerRow; // Square  grid assumption

        // Calculate the size of each cell
        float cellWidth = mapWidth / cellsPerRow;
        float cellHeight = mapHeight / cellsPerColumn;

        // Adjust positions to map's coordinate system
        float adjustedX = xPos + (mapWidth / 2);
        float adjustedY = yPos + (mapHeight / 2);

        // Calculate the indices
        int xIndex = (int)(adjustedX / cellWidth);
        int yIndex = (int)(adjustedY / cellHeight);

        // Ensure indices are within the range
        xIndex = Mathf.Clamp(xIndex, 0, cellsPerRow - 1);
        yIndex = Mathf.Clamp(yIndex, 0, cellsPerColumn - 1);

        // Calculate the final index
        return xIndex + yIndex * cellsPerRow;
    }

    Vector2 GetPartitionCenterDynamic(int partition, float mapWidth, float mapHeight, int totalPartitions)
    {
        // Calculate the number of cells per row and column, assuming a square grid
        int cellsPerRow = (int)Mathf.Sqrt(totalPartitions);
        int cellsPerColumn = cellsPerRow; // Square grid assumption

        // Calculate the size of each cell
        float cellWidth = mapWidth / cellsPerRow;
        float cellHeight = mapHeight / cellsPerColumn;

        // Calculate the row and column index of the partition
        int rowIndex = partition / cellsPerRow;
        int columnIndex = partition % cellsPerRow;

        // Calculate the center coordinates of the partition
        float centerX = (columnIndex + 0.5f) * cellWidth - (mapWidth / 2);
        float centerY = (rowIndex + 0.5f) * cellHeight - (mapHeight / 2);

        return new Vector2(centerX, centerY);
    }

    public void AddToSpatialGroup(int spatialGroupID, Enemy enemy)
    {
        enemySpatialGroups[spatialGroupID].Add(enemy);
    }

    public void RemoveFromSpatialGroup(int spatialGroupID, Enemy enemy)
    {
        enemySpatialGroups[spatialGroupID].Remove(enemy);
    }

    public void DropExperiencePoint(Vector3 position, int amount)
    {
        // Vector3 offSet = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0); // Randomize position a bit

        //GameObject expPointsGO = Instantiate(experiencePointPF, position, Quaternion.identity);
        //expPointsGO.transform.parent = experiencePointHolder;
        //ExperiencePoint xpScript = expPointsGO.GetComponent<ExperiencePoint>();

        // xpScript.Amount = amount;
        // xpScript.SpatialGroup = GetSpatialGroup(position.x, position.y);
        // xpScript.SurroundingSpatialGroups = new HashSet<int>(Utils.GetExpandedSpatialGroups(xpScript.SpatialGroup));

        // xpScript.model.transform.localPosition += offSet;
    }
}
