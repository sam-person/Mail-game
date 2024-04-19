using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomForce : MonoBehaviour
{
    public float minForce = 3f; // Minimum force magnitude
    public float maxForce = 5f; // Maximum force magnitude
    private Rigidbody rb;
    public bool isKinematicAfterTime = true;
    public float isKinematicDelay = 3f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(isKinematicAfterTime)
        {
            Invoke("SetIsKinematic", isKinematicDelay);
        }
        if (rb != null)
        {
            // Generate a random direction
            Vector3 randomDirection = Random.insideUnitSphere.normalized;

            // Generate a random force magnitude within the specified range
            float randomForceMagnitude = Random.Range(minForce, maxForce);

            // Apply force to the Rigidbody in the random direction
            rb.AddForce(randomDirection * randomForceMagnitude, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody component not found.");
        }
    }

    void SetIsKinematic()
    {
        rb.isKinematic = true;
    }
}
