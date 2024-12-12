using UnityEngine;
using TarodevController;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider2D;
    public float bounceMultiplier = 1f;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            // Ensure the collision is from above
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player != null && player.transform.position.y > transform.position.y)
            {
                player.hasToast = true;
                player.Bounce(bounceMultiplier);
                DefeatEnemy();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            // The player collided with the enemy from the side or below
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (!playerController.IsInvincible)
                {
                    playerController.TakeDamage(1);
                }
            }
        }
        else if (collision.CompareTag("Bullet"))
        {
            // Projectile hit the enemy
            DefeatEnemy();
            // Destroy the projectile to prevent multiple hits
            Destroy(collision.gameObject);
        }
    }

    private void DefeatEnemy()
    {
        // Disable the SpriteRenderer to make the enemy invisible
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Disable the Collider to prevent further collisions
        if (enemyCollider2D != null)
        {
            enemyCollider2D.enabled = false;
        }
    }
}