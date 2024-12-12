using UnityEngine;

public class Projectile : MonoBehaviour
{
  public float lifespan = 2f;
  public float ProjectileSpeed = 5;
  public SpriteRenderer zap;

  private float starttime;
  private Vector2 projectileDirection;
  private Rigidbody2D ProjectileBody;



  void Start()
  { ProjectileBody = GetComponent<Rigidbody2D>(); }

  void Update()
  {

    if ((Time.time - starttime) >= lifespan)
    {
      this.gameObject.SetActive(false);
    }

    Shoot();

  }
  void OnEnable()
  {
    starttime = Time.time;
    Vector2 direction = zap.flipX ? -transform.right : transform.right;
    projectileDirection = direction;
  }

  void Shoot()
  {


    if (ProjectileBody != null)
    {
      ProjectileBody.linearVelocity = projectileDirection * ProjectileSpeed;

    }
  }
}
