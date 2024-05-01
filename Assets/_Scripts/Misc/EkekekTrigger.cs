using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EkekekTrigger : MonoBehaviour
{
    public ScareBirdsTrigger scareBirdsTrigger;
    public bool playerInEkekekZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInEkekekZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInEkekekZone = false;
        }
    }

    private void OnDisable()
    {
        playerInEkekekZone = false;
    }

    //public void InEkekekZoneWithBirds()
    //{
    //    if(playerInEkekekZone && scareBirdsTrigger.playerInteractionHandler != null)
    //    {
    //        scareBirdsTrigger.playerInteractionHandler.Ekekek(true);
    //    }
    //}
}
