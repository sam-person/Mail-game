using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using cakeslice;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;
using StarterAssets;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Linq;

public class PlayerInteractionHandler : MonoBehaviour
{

    public static PlayerInteractionHandler instance;

    public ParticleSystem meowParticle;

    public ThirdPersonController thirdPersonController;
    public AudioSource doorSource;
    public AudioClip doorClip;
    public bool isGamePaused = false;

    public AudioClip[] meows;
    [Range(0, 1)] public float meowsVolume = 0.5f;

    private bool interactorDoorInBool = false;
    private bool interactorDoorOutBool = false;
    private bool interactorItemBool = false;
    private bool interactorDialogueBool = false;

    
    public List<GameObject> collisionObjects = new List<GameObject>();

    


    public Animator animator;

    Coroutine teleportCoroutine;
    Coroutine fadeInCoroutine;
    Coroutine fadeOutCoroutine;
    Coroutine talkingCoroutine;
    Coroutine itemCoroutine;
    Coroutine interactionAnimatorCoroutine;

    [ReadOnly]
    public TRI_Interactable closestInteractable;

    //public delegate void GamePaused(bool isPaused);
    //public static event GamePaused gamePaused;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        GameManager.gamePaused += PauseAnim;
        GameManager.gamePaused += pihIsDisabled;


    }

    public void PauseAnim(bool animPause)
    {
        if (animPause)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }

    //Calculate the closest interactable
    void CalculateClosestInteractable() {
        //If the gamemanager has no interactables, we can't find any
        if (GameManager.instance.interactables.Count == 0) {
            SetClosestInteractable(null);
            return;
        }

        //If there are none in range, we can't find any
        if (!GameManager.instance.interactables.Any(x => Vector3.Distance(x.transform.position, this.transform.position) < x.interactionRange)) {
            SetClosestInteractable(null);
            return;
        }

        //Take the closest interactable that we have in range, that's our closest.
        TRI_Interactable foundInteractable = GameManager.instance.interactables.Where(x => Vector3.Distance(x.transform.position, this.transform.position) < x.interactionRange).OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position)).First();
        SetClosestInteractable(foundInteractable);


    }

    void SetClosestInteractable(TRI_Interactable interactable) {
        //if the new interactable is the same as the old interactable, do nothing.
        if (closestInteractable == interactable) return;

        //if we already have an interactable, we need to turn off its outline
        if (closestInteractable != null) {
            closestInteractable.outline.enabled = false;
        }

        //if the new interactable isn't null, we need to set it up
        if (interactable != null) {
            //set up the new interactable
            interactable.outline.enabled = true;
        }
        
        closestInteractable = interactable;
    }

    public void Teleport(REC_Teleport teleport) {
        StartCoroutine(TeleportPlayer(teleport));
        InterfaceManager.instance.fader.Fade();
    }

    private IEnumerator TeleportPlayer(REC_Teleport door)
    {
        if (door.inside)
        {
            yield return new WaitForSeconds(0.2f);
            transform.rotation = door.spawnPoint.transform.rotation;
            transform.position = door.spawnPoint.transform.position;
            thirdPersonController.SetCameraAngle(door.xRot, door.yRot);
            VrCamForceUpdate.VrCamForceResetAction?.Invoke();
            GameManager.instance.mainCamera.clearFlags = CameraClearFlags.SolidColor;
            animator.SetBool("interact", false);
            doorSource.PlayOneShot(doorClip);
        }

        else 
        {
            yield return new WaitForSeconds(0.2f);
            transform.rotation = door.spawnPoint.transform.rotation;
            transform.position = door.spawnPoint.transform.position;
            thirdPersonController.SetCameraAngle(door.xRot, door.yRot);
            VrCamForceUpdate.VrCamForceResetAction?.Invoke();
            GameManager.instance.mainCamera.clearFlags = CameraClearFlags.Skybox;
            animator.SetBool("interact", false);
            doorSource.PlayOneShot(doorClip);
        }
    }
    public void PlayInteractionAnim()
    {
        interactionAnimatorCoroutine = StartCoroutine(InteractionAnimator());
        animator.SetBool("interact", true);

    }
    private IEnumerator InteractionAnimator()
    {
            yield return new WaitForSeconds(0.2f);
            animator.SetBool("interact", false);     
    }


    public void CattoInteractor()
    {
        if (closestInteractable) {
            closestInteractable.Activate();
        }
        /*
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
            
            //InterfaceManager.instance.DialogueController(true);           
            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
            
        }

        if (interactorItemBool)
        {

            interactionAnimatorCoroutine = StartCoroutine(InteractionAnimator());
            Debug.Log("currentItem is equal to " + currentItem);
            currentItemGO.Interact(true);
            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
            animator.SetBool("interact", true);

        }
        */

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

        //for the time being, check for closest interactable every frame
        CalculateClosestInteractable();
    }

    //private void OnTriggerEnter(Collider other)
    //{


    //    switch (other.tag)
    //    {
    //        case "InteractableDoorIn":
    //            collisionObjects.Add(other.gameObject);
    //            Debug.Log("Added object to focused list: " + other);
    //            interactorDoorInBool = true;
    //            Debug.Log("interactor focused on a Door In");
    //            focusCollider = other;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = true;
    //            break;

    //        case "InteractableDoorOut":
    //            collisionObjects.Add(other.gameObject);
    //            Debug.Log("Added object to focused list: " + other);
    //            Debug.Log("interactor focused on a Door Out");
    //            focusCollider = other;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = true;
    //            interactorDoorOutBool = true;
    //            break;
            
    //        case "InteractableDialogue":
    //            collisionObjects.Add(other.gameObject);
    //            Debug.Log("Added object to focused list: " + other);
    //            Debug.Log("interactor focused on a person");
    //            focusCollider = other;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = true;

    //            InterfaceManager.instance.currentChar = other.GetComponent<CharacterScript>();

    //            interactorDialogueBool = true;
    //            break;
            
    //        case "InteractableItem":

    //            IInteractable currentItem = other.gameObject.GetComponent<IInteractable>();
    //            currentItemGO = currentItem;

    //            collisionObjects.Add(other.gameObject);
    //            Debug.Log("Added object to focused list: " + other);
    //            Debug.Log("interactor focused on an item");
    //            focusCollider = other;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = true;
    //            interactorItemBool = true;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    List<GameObject> toDelete = new List<GameObject>();

    //    foreach (GameObject gameObject in collisionObjects)
    //    {
    //        toDelete.Add(gameObject);
    //    }

    //    foreach (var collider in toDelete)
    //    {
    //        collider.gameObject.GetComponent<Outline>().enabled = false;
    //        collisionObjects.Remove(collider);
    //        Debug.Log("Removed object from focused list: " + other);
    //    }

    //    switch (other.tag)
    //    {
    //        case "InteractableDoorIn":
    //            Debug.Log("interactor stop focused on a Door In");
    //            interactorDoorInBool = false;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
    //            break;
    //        case "InteractableDoorOut":
    //            Debug.Log("interactor stop focused on a Door Out");
    //            interactorDoorOutBool = false;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
    //            break;
    //        case "InteractableDialogue":
    //            Debug.Log("interactor stop focused on a person");
    //            interactorDoorInBool = false;
    //            InterfaceManager.instance.currentChar = null;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
    //            break;
    //        case "InteractableDItem":
    //            Debug.Log("interactor stop focused on an item");
    //            interactorDoorInBool = false;
    //            focusCollider.gameObject.GetComponent<Outline>().enabled = false;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void pihIsDisabled(bool isDisabled)
    {
        if (isDisabled)
        {
            this.enabled = false;
            thirdPersonController.enabled = false;
        }
        else
        {
            this.enabled = true;
            thirdPersonController.enabled = true;
        }
    }
}
