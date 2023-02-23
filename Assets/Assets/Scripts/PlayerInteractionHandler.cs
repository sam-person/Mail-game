using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using cakeslice;
public class PlayerInteractionHandler : MonoBehaviour
{

    public GameObject player;
    private bool interactorFocused = false;
    private Collider focusCollider;
    


    private void TeleportPlayer(Door door)
    {
        player.transform.rotation = door.partnerDoor.transform.rotation;
        player.transform.position = door.partnerDoor.transform.position + Vector3.forward;
        
    }


    private void Update()
    {
        if (interactorFocused)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TeleportPlayer(focusCollider.gameObject.GetComponent<Door>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.tag == "Interactable")
        {

            focusCollider = other;
            focusCollider.gameObject.GetComponent<Outline>().enabled = true;


            interactorFocused = true;


            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            interactorFocused = false;
            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

}
