using UnityEngine;

namespace TTT.System
{
    public class CoinsController : MonoBehaviour
    {
        [Header("Floating Settings")]
        public float floatAmplitude = 0.5f; // How far the coin floats up and down
        public float floatFrequency = 1f; // How fast the coin floats
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

                //Plays the udio of collecting the audio
                AudioManager.instance.PlayCoinSound();
                // Destroy the coin object
                Destroy(gameObject);
            }
        }
    }
}

