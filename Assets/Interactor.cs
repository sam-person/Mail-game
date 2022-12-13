using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{

    [SerializeField] private UnityEvent myTrigger;

    public bool currentlyColliding = false;


    private void Update()
    {
        if (currentlyColliding)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                myTrigger.Invoke();
                Debug.Log("I pressed e");
                
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {

            print("ENTER");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            currentlyColliding = true;
            //print("STAY");
        }
    }

    void OnTriggerExit(Collider other)

    {

        if (other.gameObject.tag == "Interactable")

        {
            currentlyColliding = false;
            //print("EXIT");

        }

    }


}

