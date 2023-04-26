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

    public Animator animator;

    Coroutine teleportCoroutine;
    Coroutine fadeInCoroutine;
    Coroutine fadeOutCoroutine;

    private IEnumerator TeleportPlayer(Door door)
    {
        yield return new WaitForSeconds(0.2f);
        player.transform.rotation = door.spawnPoint.transform.rotation;
        player.transform.position = door.spawnPoint.transform.position;


    }


    private void Update()
    {
        if (interactorFocused)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                fadeInCoroutine = StartCoroutine(UIController.FadeBlackOutSquare());
                teleportCoroutine = StartCoroutine(TeleportPlayer(focusCollider.gameObject.GetComponent<Door>()));
                //fadeOutCoroutine = StartCoroutine(UIController.FadeBlackOutSquare(false));

                animator.SetBool("interact", true);
            }

            else
            {
                animator.SetBool("interact", false);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.tag == "Interactable")
        {
            Debug.Log("interactor focused");
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
