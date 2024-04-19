using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWaterFloatingEffect : MonoBehaviour
{
    private bool isInWater = false;
    private Rigidbody rb;
    private float waterLevel = 0f; 

    public float buoyancyStrength = 0.5f; 
    public float dragAmount = 0.5f; 
    public float angularDrag = 0.5f; 
    public float rotationSpeed = 2f; // Speed of rotation to upright position
    public float movementSpeed = 1f; 

    private Vector3 waterSurfacePosition; 
    private Vector3 lastCollisionPoint; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            float depth = waterSurfacePosition.y - transform.position.y;

            // Check if the object is below the water surface
            if (depth > 0)
            {
                float buoyantForceMagnitude = buoyancyStrength * depth;

                // Apply the buoyant force to the Rigidbody
                rb.AddForce(Vector3.up * buoyantForceMagnitude, ForceMode.Acceleration);

                // Apply drag force
                Vector3 dragForce = -rb.velocity * dragAmount;
                rb.AddForce(dragForce, ForceMode.Acceleration);

                // Apply angular drag
                rb.angularDrag = angularDrag;

                //// Rotate into upright position
                //Vector3 waterSurfaceNormal = Vector3.up;
                //Quaternion targetRotation = Quaternion.FromToRotation(transform.up, waterSurfaceNormal) * transform.rotation;
                //rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

                // Calculate the rotation needed to keep the Rigidbody upright
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;

                // Smoothly interpolate the rotation towards the target rotation
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));


                // Apply collision force
                if (lastCollisionPoint != Vector3.zero)
                {
                    // go in the opposite direction of where you last collided
                    Vector3 direction = transform.position - lastCollisionPoint;
                    direction.y = 0f; // Ignore vertical component
                    rb.AddForce(direction.normalized * movementSpeed, ForceMode.Force);
                }
                else
                {
                    rb.AddForce(-transform.right * movementSpeed, ForceMode.Force);
                }
            }
            else
            {
                // Reset angular drag when above the water
                rb.angularDrag = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Water")
        {
            //get height of water
            waterLevel = other.transform.Find("WaterTop").transform.position.y;
            waterSurfacePosition = new Vector3(transform.position.x, waterLevel, transform.position.z);
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            isInWater = false;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        lastCollisionPoint = collision.contacts[0].point;
    }
}
