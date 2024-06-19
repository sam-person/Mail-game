using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapInteraction : MonoBehaviour
{
    public GameObject tapHandle;
    public float rotationSpeed = 50f;
    public float timeAlive = 3f;
    public float targetSizeFloat = 1;
    private Vector3 targetSize;
    private Vector3 initialSize = new Vector3(1f, 0.01f, 1f);

    public float duration = .5f;
    private float elapsedTime = 0f;
    public float handleDuration = .3f;
    private float t;

    private void Awake()
    {
        elapsedTime = 0f;
        t = 0f;
        transform.localScale = initialSize;
        targetSize = new Vector3(1, targetSizeFloat, 1);
    }

    private void OnDisable()
    {
        elapsedTime = 0f;
        t = 0f;
        transform.localScale = initialSize;
    }

    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the percentage of time elapsed
        t = elapsedTime / duration;

        // Lerp the scale from initial to target scale based on the elapsed time percentage
        transform.localScale = Vector3.Lerp(initialSize, targetSize, t);

        // If the elapsed time exceeds the duration, set the final scale to the target scale
        if (elapsedTime >= duration)
        {
            transform.localScale = targetSize;
        }

        if(elapsedTime >= timeAlive)
        {
            gameObject.SetActive(false);
        }

        if (tapHandle != null && elapsedTime <= handleDuration)
        {
            tapHandle.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
