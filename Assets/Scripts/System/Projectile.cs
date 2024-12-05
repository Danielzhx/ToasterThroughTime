using TarodevController;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan = 2f;
    private float starttime;
    public float ProjectileSpeed = 5;

    public Vector2 projectileDirection;
    
    private Rigidbody2D ProjectileBody;
    public PlayerController playerController;
 


    void Start()    
    {ProjectileBody = GetComponent<Rigidbody2D>();}
  
       void Update() {

        if ((Time.time - starttime) >= lifespan){
            this.gameObject.SetActive(false);     
        }

        Shoot();
       
       }
        void OnEnable(){
        starttime =  Time.time;
        Vector2 direction = playerController.isFacingRight ? transform.right : -transform.right;
        projectileDirection = direction;    
    }

       void Shoot(){


         if (ProjectileBody != null){
        ProjectileBody.linearVelocity = projectileDirection * ProjectileSpeed;

         }
       }
}
