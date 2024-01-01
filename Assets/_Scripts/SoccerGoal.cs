using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGoal : MonoBehaviour
{
    public GameObject particlesGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Can" && !particlesGO.activeSelf)
        {
            StartCoroutine(Goal());
        }
    }

    IEnumerator Goal()
    {
        particlesGO.SetActive(true);
        yield return new WaitForSeconds(2f);
        particlesGO.SetActive(false);
    }
}
