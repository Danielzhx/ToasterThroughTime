using UnityEngine;
using AudioManger;

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
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player presses the fire button (e.g., space bar) and has charges
        if (Input.GetKeyDown(KeyCode.Space) && charges.currentCharges > 0)
        {
            AudioManager.instance.PlayCharacterShootingSound(); 
            _anim.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// This method should be called via an Animation Event at the end of the shooting animation.
    /// </summary>
    void Shoot()
    {
        // Consume a charge and get the index of the consumed charge
        int consumedChargeIndex = charges.ConsumeCharge();
        if (consumedChargeIndex == -1)
        {
            Debug.LogWarning("No charges left to consume!");
            return;
        }

        if (currentProjIndex >= Projectiles.Length)
        {
            currentProjIndex = 0;
        }

        GameObject currentProjectile = Projectiles[currentProjIndex];
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();

        if (currentProjectile != null && !currentProjectile.activeSelf)
        {
            if (rb != null)
            {
                currentProjectile.SetActive(true);
                currentProjIndex++;
                currentProjectile.transform.position = firePoint.position;
                rb.linearVelocity = firePoint.right * projectileSpeed; // Assuming firePoint.right is the shooting direction
            }
        }
        else
        {
            Debug.LogWarning("Projectile is already active or missing Rigidbody2D.");
        }

        // Optional: Handle firing when no firePoint is specified
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not set. Please assign a firePoint in the Inspector.");
            return;
        }
    }

    void disableShooting()
    {
        this.enabled = false;
    }
}
