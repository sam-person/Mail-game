using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.InputSystem;

public class TRI_Focus : Trigger
{
    private GameObject player;
    private Animator playerAnimator;
    private GameObject FocusObject;
    public bool isObject = false;
    Coroutine interactionAnimator;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && FocusObject != null)
        {
            if (!isObject)
            {
                Activate();
            }
            else
            {
                playerAnimator.SetBool("interact", true);
                Activate();
                //interactionAnimatorCoroutine = StartCoroutine(InteractionAnimator());
                interactionAnimator = StartCoroutine(PlayerInteractionHandler.instance.InteractionAnimator());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {   if (other.tag == "PlayerInteractionCapsule")
        {
                FocusObject = this.gameObject;
                this.gameObject.GetComponent<Outline>().enabled = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerInteractionCapsule")
        {
                FocusObject = null;
                this.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

}
