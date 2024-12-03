using UnityEditor.Callbacks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan = 2f;
    private float starttime;
    public float ProjectileSpeed = 5;
    
    private Rigidbody2D ProjectileBody;


    void Awake()    
    {ProjectileBody = GetComponent<Rigidbody2D>();}
  
       void Update() {

        if ((Time.time - starttime) >= lifespan){
            this.gameObject.SetActive(false);     
        }

        shoot();
       
       }
    void OnEnable(){
        starttime =  Time.time;
    }

       void shoot(){

        Rigidbody2D rb = ProjectileBody;
        
        // Get the Rigidbody2D component from the projectile
        if (rb != null)
        {
            // Set the velocity of the projectile to move in the direction the player is facing
            rb.linearVelocity = transform.right * ProjectileSpeed;
        }
       }
}
