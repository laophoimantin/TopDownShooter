using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool shootRandom = false;

    // Stats
    int health = 100;
    int maxHealth = 100;
    float movementSpeed = 4f;

    // Experience & level
    long xp = 0;
    long xpFromLastLevel = 0;
    long xpToNextLevel = 0;
    int level = 0;

    // Spatial groups
    int spatialGroup = -1;
    public int SpatialGroup { get { return spatialGroup; } }

    // Takind damage from enemy
    int takeDamageEveryXFrames = 0;
    int takeDamageEveryXFramesCD = 10;
    float hitBoxRadius = 0.4f;

    // Nearest enemy position (for weapons)
    Vector2 nearestEnemyPosition = Vector2.zero;
    public Vector2 NearestEnemyPosition
    {
        get { return nearestEnemyPosition; }
        set { nearestEnemyPosition = value; }
    }

    bool noNearbyEnemies = false; // shoot randomly
    public bool NoNearbyEnemies
    {
        get { return noNearbyEnemies; }
        set { noNearbyEnemies = value; }
    }

    void Start()
    {
        spatialGroup = GameController.instance.GetSpatialGroup(transform.position.x , transform.position.y); // GET spatial group
        LevelUp();
    }

    void FixedUpdate()
    {
        Vector3 movementVector = Vector3.zero;

        // WASD to move around
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)   ) movementVector += Utils.V2toV3(new Vector2( 0,  1));
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ) movementVector += Utils.V2toV3(new Vector2(-1,  0)); 
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ) movementVector += Utils.V2toV3(new Vector2( 0, -1));
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) movementVector += Utils.V2toV3(new Vector2( 1,  0));

        transform.position += movementVector.normalized * Time.deltaTime * movementSpeed;

        // Calculate nearest enemy direction
        spatialGroup = GameController.instance.GetSpatialGroup(transform.position.x , transform.position.y); // GET spatial group
        CalculateNearestEnemyDirection();

        // Colliding with any enemy? Lose health?
        takeDamageEveryXFrames++;
        if (takeDamageEveryXFrames > takeDamageEveryXFramesCD)
        {
            CheckCollisionWithEnemy();
            takeDamageEveryXFrames = 0;
        }
    }

    void CheckCollisionWithEnemy()
    {
        List<int> surroundingSpatialGroups = Utils.GetExpandedSpatialGroups(spatialGroup);
        List<Enemy> surroundingEnemies = Utils.GetAllEnemiesInSpatialGroups(surroundingSpatialGroups);

        foreach (Enemy enemy in surroundingEnemies) 
        // foreach (Enemy enemy in GameController.instance.enemySpatialGroups[spatialGroup])
        {  
            if (enemy == null) continue;

            // float distance = Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y);
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < hitBoxRadius)
            {
                // Take damage
                ModifyHealth(-enemy.Damage);

                break;
            }
        }
    }

    void CalculateNearestEnemyDirection()
    {
        // Just checks enemies in the same spatial group
        float minDistance = 100f;
        Vector2 closestPosition = Vector2.zero;
        bool foundATarget = false;

        List<int> spatialGroupsToSearch = new List<int>() { spatialGroup };

        // No enemies in your spatial group, expand search to surrounding spatial groups
        // if (GameController.instance.enemySpatialGroups[spatialGroup].Count == 0)
        //     spatialGroupsToSearch = Utils.GetExpandedSpatialGroupsV2(spatialGroup, 6);
        spatialGroupsToSearch = Utils.GetExpandedSpatialGroupsV2(spatialGroup, 6);

        // Get all enemies
        List<Enemy> nearbyEnemies = Utils.GetAllEnemiesInSpatialGroups(spatialGroupsToSearch);

        // No nearby enemies?
        if (nearbyEnemies.Count == 0)
        {
            noNearbyEnemies = true;
        }
        else
        {
            noNearbyEnemies = false;

            // Filter thru enemies
            foreach (Enemy enemy in nearbyEnemies)
            {
                if (enemy == null) continue;

                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPosition = enemy.transform.position;
                    foundATarget = true;
                }
            }

            if (!foundATarget) // Somehow no targets found? randomize
                noNearbyEnemies = true;
            else
                nearestEnemyPosition = closestPosition;
        }
    }

    public void ModifyExperience(int amount)
    {
        xp += amount;
        //UIExperienceBar.instance.UpdateExperienceBar(xp - xpFromLastLevel, xpToNextLevel - xpFromLastLevel);

        if (xp >= xpToNextLevel) LevelUp();
    }

    public void LevelUp()
    {
        xpFromLastLevel = xpToNextLevel;
        xpToNextLevel = Utils.GetExperienceRequired(level) - xpToNextLevel;
        level++;

        //UIExperienceBar.instance.SetLevelText(level);

        //TODO: Gaining weapons?
    }

    public void ModifyHealth(int amount)
    {
        health += amount;

        //UIHealthBar.instance.UpdateBar(health, maxHealth);

        if (health <= 0)
        {
            KillPlayer();
        }
    }
    
    void KillPlayer()
    {
        //TODO: Other stuff

        Destroy(gameObject);
    }
}
