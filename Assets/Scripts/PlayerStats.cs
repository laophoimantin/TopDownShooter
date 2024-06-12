using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private MainPlayerMovement MainPlayerMovement;
    private Animator anim;

    public int playerMaxHealth = 3;
    public int playerCurrentHealth;
    public float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 3f;

    void Start()
    {
        invincibilityTimer = 0f;
        playerCurrentHealth = playerMaxHealth;
        MainPlayerMovement = GetComponent<MainPlayerMovement>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        if(playerCurrentHealth <= 0)
        {
            Debug.Log("player Dead!");
            if(GetComponent<Rigidbody2D>() != null)
            {
                Destroy(GetComponent<Rigidbody2D>());
            } 
        }
        anim.SetFloat("CurrentHealth", playerCurrentHealth);

        if(invincibilityTimer < 0)
        {
            anim.SetBool("Hurt", false);
        }
        else
        {
            anim.SetBool("Hurt", true);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Mob") || collision.CompareTag("MobBullet")) && playerCurrentHealth > 0 && invincibilityTimer <= 0)
        {
            playerCurrentHealth -= 1;
            Debug.Log("Get Hit! Health remain: " + playerCurrentHealth);
            invincibilityTimer = invincibilityDuration;
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            MainPlayerMovement.forceToApply += new Vector2(knockbackDirection.x * 20f, knockbackDirection.y * 20f);
        }
    }

}
