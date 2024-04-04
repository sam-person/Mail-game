using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    public Transform targetPoint; // The point to move towards
    public float speed = 5f; // Speed of movement

    private void Update()
    {
        if (targetPoint != null)
        {
            // Calculate the direction to move towards the target point
            Vector3 direction = targetPoint.position - transform.position;

            // Normalize the direction to get a unit vector
            direction.Normalize();

            // Move the GameObject towards the target point
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
