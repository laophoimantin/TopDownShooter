using UnityEngine;

public class AdvancedMobController : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    private AdvancedGunController gunController;
    private Rigidbody2D rb;

    [Header("Basic Data")]
    [SerializeField] private MobData MobDta;
    private float currentHealth;
    private float fireRateTimer;

    [Header("Knock Back")]
    private Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;

    [Header("Visual")]
    private SpriteRenderer visual;
    private SoundManager audioManager;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gunController = player.GetComponent<AdvancedGunController>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MobDta.mobHealth;

        visual = GetComponent<SpriteRenderer>();
        visual.sprite = MobDta.Image;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (currentHealth > 0)
            {
                if (player != null)
                {
                    if (currentHealth > 0 && MobDta.ranger)
                    {
                        fireRateTimer -= Time.deltaTime;
                        Shoot();
                    }
                    Flip();
                }
            }
            else
            {
                Die();
            }

            // Teleport the monster if the player runs too far
            if (Vector2.Distance(transform.position, player.transform.position) >= MobDta.despawnDistance)
            {
                ReturnEnemy();
            }
        }
    }

    private void ReturnEnemy()
    {
        if (player != null)
        {
            EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
            enemySpawner.UpdateAvailableSpawnPoints();
            transform.position = enemySpawner.availSpawnPoints[Random.Range(0, enemySpawner.availSpawnPoints.Count)].position;
        }
    }

    void FixedUpdate()
    {
        if (player != null && currentHealth > 0)
        {
            MoveTowardsPlayer();
        }
    }

    private void Flip()
    {
        if (player.transform.position.x < transform.position.x)
        {
            visual.flipX = true;
        }
        else if (player.transform.position.x > transform.position.x)
        {
            visual.flipX = false;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject death = Instantiate(MobDta.deathAnim, transform.position, Quaternion.identity);
        Destroy(death, 3);
    }

    private void MoveTowardsPlayer()
    {
        if (forceToApply == Vector2.zero)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, MobDta.mobSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = forceToApply;
            forceToApply /= forceDamping;
            if (Mathf.Abs(forceToApply.x) <= 0.1f && Mathf.Abs(forceToApply.y) <= 0.1f)
            {
                forceToApply = Vector2.zero;
            }
        }
    }

    private void Shoot()
    {
        if (fireRateTimer <= 0)
        {
            audioSource.PlayOneShot(audioManager.monsterShootClip);
            GameObject bullet = Instantiate(MobDta.bulletPf, transform.position, transform.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = (player.transform.position - transform.position).normalized;
            bulletRb.velocity = direction * MobDta.bulletSpeed;
            fireRateTimer = MobDta.fireRate;
        }
    }

    public void GetHit()
    {
        if (player != null)
        {
            Instantiate(MobDta.blood, transform.position, Quaternion.identity);
            currentHealth -= gunController.damage;
            Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
            float modifiedKnockback = gunController.gunData.knockbackForce * MobDta.knockbackResistance;
            if (modifiedKnockback < 0)
            {
                modifiedKnockback = 0;
            }
            forceToApply += new Vector2(knockbackDirection.x * modifiedKnockback, knockbackDirection.y * modifiedKnockback);
        }
    }

    private void OnDestroy()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyKilled();
        }
    }
}
