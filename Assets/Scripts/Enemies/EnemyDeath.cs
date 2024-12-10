using UnityEngine;
using TarodevController;

public class EnemyDeath : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            // The player hit the enemy with their feet, process bounce and defeat enemy
            PlayerController playerController = collision.GetComponentInParent<PlayerController>();
            if (playerController != null && !playerController.IsInvincible)
            {
                playerController.Bounce(); // Uses the bounceMultiplier
                playerController.SetJustBounced(0.2f); // Sets the flag for 0.2 seconds
                DefeatEnemy();
            }

            return;
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
