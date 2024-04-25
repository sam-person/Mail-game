using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGoal : MonoBehaviour
{
    public GameObject particlesGO;
    public float particleLifeTime = 3f;
    public LayerMask layerToCheck;

    private void OnTriggerEnter(Collider other)
    {
        if (layerToCheck == (layerToCheck | (1 << other.gameObject.layer)) && !particlesGO.activeSelf)
        {
            StartCoroutine(Goal());
        }
    }

    IEnumerator Goal()
    {
        particlesGO.SetActive(true);
        yield return new WaitForSeconds(particleLifeTime);
        particlesGO.SetActive(false);
    }
}
