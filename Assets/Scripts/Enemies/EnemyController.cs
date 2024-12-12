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

   void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player == null) return;

            bool stomped = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) {
                stomped = true;
                break;
            }
            }

            if (stomped)
            {
                // Player landed on top
                player.hasToast = true;
                player.Bounce(bounceMultiplier);
                DefeatEnemy();
            }
            else
            {
                // Player hit from side or below
                if (!player.IsInvincible)
                {
                    player.TakeDamage(1);
                }
            }
        }
        if (collision.gameObject.CompareTag("Bullet")) {
        DefeatEnemy();
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