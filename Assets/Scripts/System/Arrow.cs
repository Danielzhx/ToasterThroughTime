using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform pivot;
    public float anglePosition0 = 0f; // Angle for position 0
    public float anglePosition1 = 90f; // Angle for position 1
    public float anglePosition2 = 180f; // Angle for position 2
    public float anglePosition3 = 270f; // Angle for position 3
    public int targetPositionIndex = 0; // Target position index (0 to 3)
    public float rotationSpeed = 5f; // Speed of rotation

    void Update()
    {
        if (pivot == null) return;

        // Determine the target rotation based on the target position index
        float targetRotation = 0f;
        switch (targetPositionIndex)
        {
            case 1:
                targetRotation = anglePosition0;
                break;
            case 2:
                targetRotation = anglePosition1;
                break;
            case 3:
                targetRotation = anglePosition2;
                break;
            case 4:
                targetRotation = anglePosition3;
                break;
        }

        // Smoothly rotate the arrow around the pivot to the target rotation
        float currentRotation = Mathf.LerpAngle(transform.eulerAngles.z, targetRotation, rotationSpeed * Time.deltaTime);
        transform.RotateAround(pivot.position, Vector3.forward, currentRotation - transform.eulerAngles.z);
    }

    // Method to set the target position index based on health value
    public void SetHealthPosition(int health)
    {
        // Assuming health ranges from 0 to 4
        targetPositionIndex = Mathf.Clamp(4 - health, 1, 4);
    }
}
