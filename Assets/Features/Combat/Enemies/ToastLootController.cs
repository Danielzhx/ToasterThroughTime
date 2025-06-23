using UnityEngine;
using TTT.System;

namespace TTT
{
    public class ToastLootController : MonoBehaviour
    {
        [Header("Floating Settings")]
        public float floatAmplitude = 0.3f; // How far the coin floats up and down
        public float floatFrequency = 1f; // How fast the coin floats
        public bool isCollected = false;

        private Vector3 _startPosition;
        [SerializeField] private PlayerController playerController;

        public void ResetStartPosition(Vector3 newPos)
        {
            _startPosition = newPos;
            transform.position = newPos;
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
            if (collision.CompareTag("Player") && !playerController.hasToast)
            {
                isCollected = true;
                playerController.hasToast = true;
                // Destroy the coin object
                gameObject.SetActive(false);
            }
        }
    }
}

