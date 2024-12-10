using TarodevController;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Reference to the projectile prefab
    public GameObject projectilePrefab;

    // Shooting force to apply to the projectile
    public float projectileSpeed = 10f;

    // Offset for where to instantiate the projectile
    public Transform firePoint;
    public Charges charges;

    public GameObject[] Projectiles;

    private int currentProjIndex = 0;

    private PlayerController playerController;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player presses the fire button (e.g., left mouse button or space)
        if (Input.GetButtonDown("Fire1") && charges.currentCharges > 0)
        {
            _anim.SetTrigger("Attack");
        }
    }

    void Shoot()
    {
        if (currentProjIndex >= Projectiles.Length)
        {
            currentProjIndex = 0;
        }


        GameObject currentProjectile = Projectiles[currentProjIndex];
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();

        if (!currentProjectile.gameObject.activeSelf)
        {
            if (rb != null)
            {
                currentProjectile.SetActive(true);
                currentProjIndex++;
                currentProjectile.transform.position += firePoint.position - currentProjectile.transform.position;
                charges.currentCharges--;
            }
        }

        // If no firePoint is specified, use the player position as the spawn point
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not set. Please assign a firePoint in the Inspector.");
            return;
        }
    }
}
