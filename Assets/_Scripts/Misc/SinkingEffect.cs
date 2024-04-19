using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingEffect : MonoBehaviour
{
    public float delay = 2f; // Delay before the movement starts
    public float duration = 1f; // Duration of the movement
    public float speed = .5f; // Speed of the movement

    private float elapsedTime = 0f; // Elapsed time since the start of the movement
    private bool isMoving = false; // Flag to track if the movement is in progress

    void Update()
    {
        // Update the elapsed time if the movement is in progress
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the position to move towards
            float distanceToMove = speed * Time.deltaTime;
            transform.Translate(Vector3.down * distanceToMove, Space.World);

            // Check if the movement duration has passed
            if (elapsedTime >= duration)
            {
                gameObject.SetActive(false);
                isMoving = false;
            }
        }
    }

    void Start()
    {
        // Start the movement after the delay
        Invoke("StartMoving", delay);
    }

    void StartMoving()
    {
        isMoving = true;
    }
}
