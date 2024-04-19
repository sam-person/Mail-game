using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PushObject : MonoBehaviour
{
    public float pushForce = 10f; // The force to apply to the object.
    //public KeyCode pushButton = KeyCode.E; // The button to trigger the push.

    private bool playerInRange = false; //track player proximity.

    private GameObject player;

    private Rigidbody rb;
    [SerializeField] private float physicsDurationCountdown = 5f;
    private Coroutine physicsCoroutine;
    private PlayerInteractionHandler playerScript;
    private bool interactionButtonPressed = false;

    private bool isBreakable;
    private BreakableObject breakableObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerInteractionHandler>();
        if(gameObject.GetComponent<BreakableObject>())
        {
            isBreakable = true;
            breakableObject = gameObject.GetComponent<BreakableObject>();
        }
    }

    private void Update()
    {
        if (playerScript._input.interact && !interactionButtonPressed && playerInRange)
        {
            Push();
            interactionButtonPressed = true;
        }
        else if (!playerScript._input.interact)
        {
            interactionButtonPressed = false;
        }
    }

    private void Push()
    {
        if(isBreakable)
        {
            StartCoroutine(MakeBreakable());
        }
        rb.isKinematic = false;
        //if (physicsCoroutine != null)
        //{
        //    StopCoroutine(physicsCoroutine);
        //    physicsCoroutine = null;
        //}
        Vector3 pushDirection = player.transform.forward;
        rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        playerScript.PlayInteractionAnim();

       // physicsCoroutine ??= StartCoroutine(DisablePhysics());
    }

    //delay added so it doesn't immediately break from the contact of its current placement
    private IEnumerator MakeBreakable()
    {
        yield return new WaitForSeconds(.2f);
        breakableObject.canBeBroken = true;
    }


    //private IEnumerator DisablePhysics()
    //{
    //    yield return new WaitForSeconds(physicsDurationCountdown);
    //   // rb.isKinematic = true;
    //    physicsCoroutine = null;
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("within range of pushable object");
            //player = other.gameObject;
            //playerScript = player.GetComponent<PlayerInteractionHandler>();
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}