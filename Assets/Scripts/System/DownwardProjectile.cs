using UnityEngine;
public class DownwardProjectile : MonoBehaviour
{
    /*

    // Version 1: WITH Debug Gizmos
    // ----------------------------------------------------------
    // This version draws a Gizmo at the collision impact point.

    public float fallSpeed = 10f;
    public float aoeRadius = 2f;
    public bool showRadius = true;
    public Color radiusColor = Color.red;

    private Rigidbody2D rb;
    private bool hasImpacted = false;
    private Vector2 impactPoint;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update() {
        if (!hasImpacted) {
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        } else {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Check if we hit ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            PerformAOE();

            // Store the impact point
            impactPoint = collision.GetContact(0).point;
            hasImpacted = true;

            // Hide the projectile visually after impact
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }
    }

    void PerformAOE() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (Collider2D hit in hits) {
            if (hit.CompareTag("Enemy")) {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null) {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnDrawGizmos() {
        if (showRadius && hasImpacted) {
            Gizmos.color = radiusColor;
            Gizmos.DrawWireSphere(impactPoint, aoeRadius);
        }
    }
    */


    
    // Version 2: WITHOUT Debug Gizmos
    // ----------------------------------------------------------
    // This version does not draw any Gizmos. If you want to switch to this version,
    // comment out the code above and uncomment this entire block.
    
    public float fallSpeed = 10f;
    public float aoeRadius = 2f;

    private Rigidbody2D rb;
    private bool hasImpacted = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update() {
        if (!hasImpacted) {
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        } else {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            PerformAOE();

            hasImpacted = true;

            // Hide the projectile visually after impact
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }
    }

    void PerformAOE() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (Collider2D hit in hits) {
            if (hit.CompareTag("Enemy")) {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null) {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }
    
}
