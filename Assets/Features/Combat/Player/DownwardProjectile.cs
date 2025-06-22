using UnityEngine;

namespace TTT
{
    public class DownwardProjectile : MonoBehaviour
    {
    
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

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.CompareTag("Enemy")) {
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
        /*
        void OnDrawGizmos() {
            if (showRadius && hasImpacted) {
                Gizmos.color = radiusColor;
                Gizmos.DrawWireSphere(impactPoint, aoeRadius);
            }
        }
        */
    }
}

