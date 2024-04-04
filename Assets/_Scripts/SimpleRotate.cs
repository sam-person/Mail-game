using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 45, 0); // Rotation speed in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Rotate the GameObject around its own axis
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
