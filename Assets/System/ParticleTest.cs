using UnityEngine;

public class TestParticleEffect : MonoBehaviour
{
    public GameObject particlePrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Press P to test
        {
            if (particlePrefab != null)
            {
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Particle prefab not assigned!");
            }
        }
    }
}
