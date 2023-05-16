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

    public static PlayerInteractionHandler instance;

    public ParticleSystem meowParticle;
    public GameObject player;
    public Collider focusCollider;
    public FadeController FadeController;
    public Camera mainCamera;
    public CinemachineTargetGroup targetGroup;
    public GameObject dialogueCamera;

    public ThirdPersonController thirdPersonController;
    public PlayerInteractionHandler playerInteractionHandler;
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
    Coroutine talkingCoroutine;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerInteractionHandler = GetComponent<PlayerInteractionHandler>();
        
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
            talkingCoroutine = StartCoroutine(InterfaceManager.instance.DialogueController(true));
            //InterfaceManager.instance.DialogueController(true);           
            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
            
        }

    }


    public  void MeowButton()
    {
        var index = Random.Range(0, meows.Length);
        AudioSource.PlayClipAtPoint(meows[index], this.transform.position, meowsVolume);
        meowParticle.Emit(1); 
    }

    public void MeowConversation()
    {
        var index = Random.Range(0, meows.Length);
        AudioSource.PlayClipAtPoint(meows[index], this.transform.position, meowsVolume);
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

                InterfaceManager.instance.currentChar = other.GetComponent<CharacterScript>();

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
                InterfaceManager.instance.currentChar = null;
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
