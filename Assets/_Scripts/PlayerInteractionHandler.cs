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

    public float interactionCooldown = 2f;
    float _interactionCooldown;


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
        StartInteractionCooldown(); //make sure this logic starts if nothing else starts it, might not be needed

    }

    void HandleInteractionCooldown() {
        if (_interactionCooldown > 0f) {
            _interactionCooldown -= Time.deltaTime;
            if (_interactionCooldown <= 0f) {
                _interactionCooldown = 0f;
            }
        }
    }

    public void StartInteractionCooldown(float customInteractionCooldown = 0f) {
        //if you're using the default value
        if (customInteractionCooldown == 0f)
        {
            _interactionCooldown = interactionCooldown;
        }
        else {//if you're using a custom valu 
            _interactionCooldown = customInteractionCooldown;
        }
    }

    //Calculate the closest interactable
    void CalculateClosestInteractable() {
        //check if we're on interactable cooldown
        if (_interactionCooldown > 0f) {
            SetClosestInteractable(null);
            return;
        }


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

        //check it's state, if it's inactive, ignore it.
        //would this be better handled in the range check, so we don't invalidate a second, in range interactable by standing next to an inactive one?
        if (foundInteractable.interactionState == TRI_Interactable.InteractionType.Inactive) {
            SetClosestInteractable(null);
            return;
        }
        
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
            if (interactable.getIsValid())
            {
                interactable.outline.enabled = true;
            }
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
            StartInteractionCooldown();
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
            animator.SetTrigger("interact");
            CattoInteractor();
        }

        if (Input.GetMouseButtonDown(0))
        {
            MeowButton();
        }

        //for the time being, check for closest interactable every frame
        switch (GameManager.instance.gameState)
        {
            case GameManager.GameState.Gameplay:
                HandleInteractionCooldown();
                CalculateClosestInteractable();
                break;
            case GameManager.GameState.Dialogue:
                break;
            case GameManager.GameState.Paused:
                break;
        }
        
    }



}
