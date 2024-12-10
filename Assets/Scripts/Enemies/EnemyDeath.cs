using System;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {

    public EnemyMovement enemy;

void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Bullet"))
    {
        // Disable the SpriteRenderer to make the enemy invisible
        GetComponent<SpriteRenderer>().enabled = false;

        // Disable the Collider to prevent further collisions
        GetComponent<Collider2D>().enabled = false;
    }
}









}