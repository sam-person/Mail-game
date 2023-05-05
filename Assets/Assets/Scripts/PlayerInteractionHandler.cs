using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using cakeslice;
using Unity.VisualScripting;

public class PlayerInteractionHandler : MonoBehaviour
{

    public GameObject player;
    private Collider focusCollider;
    public UIController UIController;
    public Camera mainCamera;

    public AudioSource doorSource;
    public AudioClip doorClip;


    private bool interactorDoorInBool = false;
    private bool interactorDoorOutBool = false;
    private bool interactorItemBool = false;
    private bool interactorDialogueBool = false;


    public Animator animator;

    Coroutine teleportCoroutine;
    Coroutine fadeInCoroutine;
    Coroutine fadeOutCoroutine;

    private IEnumerator TeleportPlayer(Door door)
    {
        if (door.tag == "InteractableDoorIn")
        {
            yield return new WaitForSeconds(0.2f);
            player.transform.rotation = door.spawnPoint.transform.rotation;
            player.transform.position = door.spawnPoint.transform.position;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            animator.SetBool("interact", false);
            doorSource.PlayOneShot(doorClip);

        }

        else 
        {
            yield return new WaitForSeconds(0.2f);
            player.transform.rotation = door.spawnPoint.transform.rotation;
            player.transform.position = door.spawnPoint.transform.position;
            mainCamera.clearFlags = CameraClearFlags.Skybox;
            animator.SetBool("interact", false);
            doorSource.PlayOneShot(doorClip);

        }

    }


    public void CattoInteractor()
    {
        if (!interactorDoorInBool && !interactorDoorOutBool && !interactorItemBool && !interactorDialogueBool)
        {
            return;
        }

        if (interactorDoorInBool)
        {
            fadeInCoroutine = StartCoroutine(UIController.FadeBlackOutSquare());
            teleportCoroutine = StartCoroutine(TeleportPlayer(focusCollider.gameObject.GetComponent<Door>()));
            animator.SetBool("interact", true);
            
        }

        if (interactorDoorOutBool)
        {
            fadeInCoroutine = StartCoroutine(UIController.FadeBlackOutSquare());
            teleportCoroutine = StartCoroutine(TeleportPlayer(focusCollider.gameObject.GetComponent<Door>()));
            animator.SetBool("interact", true);

        }


    }

    private void Update()
    {

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CattoInteractor();
        }


        
    }

    private void OnTriggerEnter(Collider other)
    {



        switch (other.tag)
        {
            case "InteractableDoorIn":
                interactorDoorInBool = true;
                Debug.Log("interactor focused on a Door In");
                focusCollider = other;
                focusCollider.gameObject.GetComponent<Outline>().enabled = true;
                break;

            case "InteractableDoorOut":
                Debug.Log("interactor focused on a Door Out");
                focusCollider = other;
                focusCollider.gameObject.GetComponent<Outline>().enabled = true;
                interactorDoorOutBool = true;
                break;
            
            case "InteractableDialogue":
                Debug.Log("interactor focused on a person");
                focusCollider = other;
                focusCollider.gameObject.GetComponent<Outline>().enabled = true;
                interactorDialogueBool = true;
                break;
            
            case "InteractableItem":
                Debug.Log("interactor focused on an item");
                focusCollider = other;
                focusCollider.gameObject.GetComponent<Outline>().enabled = true;
                interactorItemBool = true;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {


        switch (other.tag)
        {
            case "InteractableDoorIn":
                Debug.Log("interactor stop focused on a Door In");
                interactorDoorInBool = false;
                focusCollider.gameObject.GetComponent<Outline>().enabled = false;
                break;
            case "InteractableDoorOut":
                Debug.Log("interactor stop focused on a Door Out");
                interactorDoorOutBool = false;
                focusCollider.gameObject.GetComponent<Outline>().enabled = false;
                break;
            case "InteractableDialogue":
                Debug.Log("interactor stop focused on a person");
                interactorDoorInBool = false;
                focusCollider.gameObject.GetComponent<Outline>().enabled = false;
                break;
            case "InteractableDItem":
                Debug.Log("interactor stop focused on an item");
                interactorDoorInBool = false;
                focusCollider.gameObject.GetComponent<Outline>().enabled = false;
                break;
            default:
                break;
        }


    }

}
