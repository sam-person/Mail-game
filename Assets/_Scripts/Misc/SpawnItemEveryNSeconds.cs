using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemEveryNSeconds : MonoBehaviour
{
    public GameObject objectToInstantiate;
    public int numberOfInstances = 5;
    public float delayBetweenInstances = 1f;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private System.Collections.IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            Instantiate(objectToInstantiate, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(delayBetweenInstances);
        }
    }
}
