using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float pushForce = 10f; // The force to apply to the object.
    public KeyCode pushButton = KeyCode.E; // The button to trigger the push.

    private bool playerInRange = false; // Flag to track player proximity.

    private GameObject player;

    [Tooltip("Gets Rigidbody component.")] private Rigidbody rb;
    [SerializeField] private float physicsDuration = 5f;
    [SerializeField] private float physicsDurationCountdown;
    private Coroutine physicsCoroutine;
    private LayerMask characterLayer;
    [SerializeField] private float playerDistanceCheck;
    private PlayerInteractionHandler playerScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        physicsDurationCountdown = physicsDuration;
        characterLayer = LayerMask.GetMask("Character");
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerInteractionHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pushButton) && playerInRange)
        {
            //DistanceCheck();
            Push();
        }
    }

    //private void DistanceCheck()
    //{
    //    // Get the direction from this object to the player
    //    Vector3 directionToPlayer = player.transform.position - transform.position;
    //    // Cast a line from this object's position to the player's position
    //    Ray ray = new Ray(transform.position, directionToPlayer);

    //    // Perform the linecast
    //    if (Physics.Raycast(ray, out RaycastHit hit, playerDistanceCheck))
    //    {
    //        if (physicsCoroutine != null)
    //        {
    //            StopCoroutine(physicsCoroutine);
    //            physicsCoroutine = null;
    //        }
    //        Push();
    //        Debug.Log("Hit object");
    //    }
    //}

    private void Push()
    {
        rb.isKinematic = false;
        if (physicsCoroutine != null)
        {
            StopCoroutine(physicsCoroutine);
            physicsCoroutine = null;
        }
        Vector3 pushDirection = transform.position - player.transform.position;
        rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        playerScript.PlayInteractionAnim();

        physicsCoroutine ??= StartCoroutine(DisablePhysics());
    }

    private IEnumerator DisablePhysics()
    {
        yield return new WaitForSeconds(physicsDurationCountdown);
        rb.isKinematic = true;
        physicsCoroutine = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
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