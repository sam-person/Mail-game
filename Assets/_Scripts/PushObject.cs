using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PushObject : MonoBehaviour
{
    public float pushForce = 10f; // The force to apply to the object.
    public KeyCode pushButton = KeyCode.E; // The button to trigger the push.

    private bool playerInRange = false; // Flag to track player proximity.

    private GameObject player;

    private Rigidbody rb;
    [SerializeField] private float physicsDurationCountdown = 5f;
    private Coroutine physicsCoroutine;
    public PlayerInteractionHandler playerScript;
    private bool interactionButtonPressed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerInteractionHandler>();
    }

    private void Update()
    {
        if (playerScript._input.interact && playerInRange && !interactionButtonPressed)
        {
            Debug.Log("interact button pressed in PO");
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
        rb.isKinematic = false;
        //if (physicsCoroutine != null)
        //{
        //    StopCoroutine(physicsCoroutine);
        //    physicsCoroutine = null;
        //}
        Vector3 pushDirection = transform.position - player.transform.position;
        rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        playerScript.PlayInteractionAnim();

       // physicsCoroutine ??= StartCoroutine(DisablePhysics());
    }

    private IEnumerator DisablePhysics()
    {
        yield return new WaitForSeconds(physicsDurationCountdown);
       // rb.isKinematic = true;
        physicsCoroutine = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("within range of can");
            player = other.gameObject;
            playerScript = player.GetComponent<PlayerInteractionHandler>();
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