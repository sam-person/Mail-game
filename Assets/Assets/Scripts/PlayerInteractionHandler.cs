using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using cakeslice;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;
using StarterAssets;
using Cinemachine;

public class PlayerInteractionHandler : MonoBehaviour
{
    public ParticleSystem meowParticle;
    public GameObject player;
    private Collider focusCollider;
    public FadeController FadeController;
    public Camera mainCamera;
    public CinemachineTargetGroup targetGroup;
    public GameObject dialogueCamera;

    private ThirdPersonController thirdPersonController;

    public AudioSource doorSource;
    public AudioClip doorClip;


    public AudioClip[] meows;
    [Range(0, 1)] public float meowsVolume = 0.5f;

    private bool interactorDoorInBool = false;
    private bool interactorDoorOutBool = false;
    private bool interactorItemBool = false;
    private bool interactorDialogueBool = false;


    


    public Animator animator;

    Coroutine teleportCoroutine;
    Coroutine fadeInCoroutine;
    Coroutine fadeOutCoroutine;


    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }


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
            fadeInCoroutine = StartCoroutine(FadeController.FadeBlackOutSquare());
            teleportCoroutine = StartCoroutine(TeleportPlayer(focusCollider.gameObject.GetComponent<Door>()));
            animator.SetBool("interact", true);  
        }

        if (interactorDoorOutBool)
        {
            fadeInCoroutine = StartCoroutine(FadeController.FadeBlackOutSquare());
            teleportCoroutine = StartCoroutine(TeleportPlayer(focusCollider.gameObject.GetComponent<Door>()));
            animator.SetBool("interact", true);
        }

        if (interactorDialogueBool)
        {
            targetGroup.m_Targets[1].target = focusCollider.gameObject.transform;
            dialogueCamera.gameObject.SetActive(true);
            thirdPersonController.enabled = !thirdPersonController.enabled;
            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
        }
    }


    private void MeowButton()
    {
        var index = Random.Range(0, meows.Length);
        AudioSource.PlayClipAtPoint(meows[index], this.transform.position, meowsVolume);
        meowParticle.Emit(1); 
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CattoInteractor();
        }

        if (Input.GetMouseButtonDown(0))
        {
            MeowButton();
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
