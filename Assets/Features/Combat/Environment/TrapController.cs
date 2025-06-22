using UnityEngine;
using TarodevController;

namespace TTT
{
    public class TrapController : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 1;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player == null) return;

                if (!player.IsInvincible)
                {
                    player.TakeDamage(damageAmount);
                }
            }
        }
    }
}

