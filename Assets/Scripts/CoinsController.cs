using TMPro;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.5f; // How far the coin floats up and down
    public float floatFrequency = 1f; // How fast the coin floats
    public AudioClip collectSound;
    private Vector3 _startPosition;

    private void Start()
    {
        // Store the initial position of the coin
        _startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new position for the floating effect
        float newY = _startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with the coin
        if (collision.CompareTag("Player"))
        {
            // Add any logic for collecting the coin (e.g., incrementing a score)
            Debug.Log("Coin collected!");
            //Plays the udio of collecting the audio
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            ToastCoinsManager.instance.score += 1;
            // Destroy the coin object
            Destroy(gameObject);
        }
    }
}
