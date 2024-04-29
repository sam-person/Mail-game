using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareBirdsTrigger : MonoBehaviour
{
    public LandingSpot[] landingSpots;
    private float scareFrequency = 1f;
    private bool isInsideTrigger;


    private void OnTriggerEnter(Collider other)
    {
        isInsideTrigger = true;
        StartCoroutine(ScareBirdEveryNsecond());
    }

    private void OnTriggerExit(Collider other)
    {
        isInsideTrigger = false;
    }

    IEnumerator ScareBirdEveryNsecond()
    {
        while (true)
        {
            if(isInsideTrigger)
            {
                ScareBirds();
                yield return new WaitForSeconds(scareFrequency);
            }
            else
            {
                yield return null;
            }
        }

    }
    private void ScareBirds()
    {
        Debug.Log("Scaring birds");
        foreach (var landingspot in landingSpots)
        {
            landingspot.ReleaseFlockChild();
        }
    }
}
