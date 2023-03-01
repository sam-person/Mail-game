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
    public UIController UIController;
    


    private void TeleportPlayer(Door door)
    {
        player.transform.rotation = door.spawnPoint.transform.rotation;
        player.transform.position = door.spawnPoint.transform.position;
        
    }


    private void Update()
    {
        if (interactorFocused)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartCoroutine(UIController.FadeBlackOutSquare());
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
