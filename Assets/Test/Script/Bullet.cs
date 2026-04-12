using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool spinBullet;

    int spatialGroup = 0;

    Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection
    {
        get { return movementDirection; }
        set { movementDirection = value; }
    }

    public int bulletDamage;
    public float movementSpeed;
    public float bulletHitBoxRadius;

    public Transform modelTransform;

    // Currently surrounding spatial groups
    List<int> surroundingSpatialGroups = new List<int>();

    // ON SPAWN
    public delegate void BulletSpawnAction();
    public event BulletSpawnAction OnBulletSpawned;

    // ON TRAVEL
    public delegate void BulletTravelAction(Transform parentBullet);
    public event BulletTravelAction OnBulletTravel;

    // ON DESTROY
    public delegate void BulletDestructionAction(Transform parentBullet);
    public event BulletDestructionAction OnDestroy;

    // ON CONTACT WITH ENEMY
    public delegate void BulletEnemyContactAction(Transform parentBullet);
    public event BulletEnemyContactAction OnContactWithEnemy;

    bool isDestroyed = false;

    void Start()
    {
        spatialGroup = GameController.instance.GetSpatialGroupStatic(transform.position.x , transform.position.y); // GET spatial group
        
        // Trigger the event when the bullet is instantiated
        OnBulletSpawned?.Invoke();
    }

    void FixedUpdate()
    {
        RunLogic();
    }

    public void RunLogic()
    {
        // Move
        transform.position += Utils.V2toV3(movementDirection) * Time.deltaTime * movementSpeed;

        // Calculate the angle
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg - 90f;

        // Set the rotation of the transform
        if (!spinBullet) modelTransform.rotation = Quaternion.Euler(0, 0, angle);
        else modelTransform.Rotate(0, 0, 10f);

        // Update spatial group
        int newSpatialGroup = GameController.instance.GetSpatialGroupStatic(transform.position.x , transform.position.y); // GET spatial group
        if (newSpatialGroup != spatialGroup)
        {
            GameController.instance.bulletSpatialGroups[spatialGroup].Remove(this); // REMOVE from old spatial group

            spatialGroup = newSpatialGroup; // UPDATE current spatial group
            GameController.instance.bulletSpatialGroups[spatialGroup].Add(this); // ADD to new spatial group
            surroundingSpatialGroups = Utils.GetExpandedSpatialGroups(spatialGroup, movementDirection);
        }

        // Check collision with enemies
        CheckCollisionWithEnemy();
    }

    public void OnceASecondLogic()
    {
        // If destroyed, skip
        if (this == null) return;

        // Destroy if out of bounds
        DestroyIfOutOfBounds();
        
        OnBulletTravel?.Invoke(transform);
    }

    void CheckCollisionWithEnemy()
    {
        List<Enemy> surroundingEnemies = Utils.GetAllEnemiesInSpatialGroups(surroundingSpatialGroups);

        foreach (Enemy enemy in surroundingEnemies) 
        {  
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < bulletHitBoxRadius)
            {
                // Trigger the event when the bullet collides with an enemy
                OnContactWithEnemy?.Invoke(transform);

                // Deal damage to them
                enemy.ChangeHealth(-bulletDamage);

                // Destroy bullet
                DestroyBullet();

                break;
            }
        }
    }

    void DestroyIfOutOfBounds()
    {
        if (
            transform.position.x < GameController.instance.MAP_WIDTH_MIN ||
            transform.position.x > GameController.instance.MAP_WIDTH_MAX ||
            transform.position.y < GameController.instance.MAP_HEIGHT_MIN ||
            transform.position.y > GameController.instance.MAP_HEIGHT_MAX ||
            Vector2.Distance(transform.position, GameController.instance.player.position) > 20f
        )
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        if (isDestroyed) return;

        // Trigger the event when the bullet is destroyed
        OnDestroy?.Invoke(transform);

        //Weaponary.instance.ProjectileCount--;
        GameController.instance.bulletSpatialGroups[spatialGroup].Remove(this);
        Destroy(gameObject);
        isDestroyed = true;
    }
}
