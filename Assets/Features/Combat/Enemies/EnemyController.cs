using UnityEngine;
using TTT.System;
using System.Collections;

namespace TTT
{
    public class EnemyController : MonoBehaviour
    {
        private Collider2D enemyCollider2D;
        private Animator _anim;
        private SpriteRenderer enemyRenderer;
        private ToastLootController toastLootController;

        public GameObject toastLoot;
        public Transform[] patrolPoints;
        public float moveSpeed;
        public int patrolDestination;
        public float bounceMultiplier = 1f;


        void Start()
        {
            enemyCollider2D = GetComponent<Collider2D>();
            _anim = GetComponent<Animator>();
            enemyRenderer = GetComponent<SpriteRenderer>();
            toastLootController = toastLoot.GetComponent<ToastLootController>();
        }

        void Update()
        {
            if (patrolDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
                {
                    enemyRenderer.flipX = false;
                    patrolDestination = 1;
                }
            }

            if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
                {
                    enemyRenderer.flipX = true;
                    patrolDestination = 0;
                }
            }
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
                    if (contact.normal.y < -0.5f)
                    {
                        stomped = true;
                        break;
                    }
                }

                if (stomped)
                {

                    // Player landed on top
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

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                DefeatEnemy();
                collision.gameObject.SetActive(false);
            }
        }

        private void DefeatEnemy()
        {
            AudioManager.instance.PlayEnemyDeathSound();
            _anim.SetTrigger("KillEnemy");

            // Disable the Collider to prevent further collisions
            if (enemyCollider2D != null)
            {
                enemyCollider2D.enabled = false;
            }

            if (toastLoot != null)
            {
                toastLoot.transform.position = transform.position;
                toastLoot.SetActive(true);

                // Important: reset _startPosition so it floats from the new location
                toastLootController.ResetStartPosition(transform.position);
            }

            this.enabled = false;
            StartCoroutine(ReviveAfterTime(10f));
        }

        private IEnumerator ReviveAfterTime(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Only revive if the player hasn’t already "picked up" the enemy. 
            // (You can add logic here to check if the enemy was collected or destroyed.)

            if (!toastLootController.isCollected)
            {
                _anim.SetTrigger("ReviveEnemy");

                // Also re-enable collider or any other components so the enemy is active again
                if (enemyCollider2D != null)
                {
                    enemyCollider2D.enabled = true;
                }

                // Re-enable the script if you had disabled it 
                // (so it can patrol again, etc.)
                this.enabled = true;
                toastLoot.SetActive(false);
            }
        }
    }
}

