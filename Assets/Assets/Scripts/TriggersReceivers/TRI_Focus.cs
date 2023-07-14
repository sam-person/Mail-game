using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.InputSystem;

public class TRI_Focus : Trigger
{

    private GameObject FocusObject;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && FocusObject != null)
        {
            Activate();
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
