using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareBirdsTrigger : MonoBehaviour
{
    public LandingSpot[] landingSpots;
    public EkekekTrigger ekekekTrigger;

    [Tooltip("The frequency that we check for what is within range")]
    public float checkInterval = .5f;
    [Tooltip("Range to search within")]
    public float scareRange = 5f; 
    [Tooltip("Select the Bird and Character Mask to filter out any other collisions")]
    public LayerMask scareLayerMask;

    private float timer; // Timer to track check intervals

    Collider[] scareCollidersBuffer; // Buffer to store colliders
    const int maxColliders = 10; // Maximum number of colliders to detect

    private bool anyBirdsInRange = false;
    private bool hasFoundBirds = false;
    private bool isPlayerInRange = false;
    private bool hasFoundPlayer = false;
    [SerializeField]
    private bool isFacingBirds = false;
    [Tooltip("0 = 180 degree view")]
    public float facingBirdsFov = .1f;

    [HideInInspector]
    public PlayerInteractionHandler playerInteractionHandler;

    void Awake()
    {
        // Initialize the colliders buffer
        scareCollidersBuffer = new Collider[maxColliders];
        playerInteractionHandler = FindObjectOfType<PlayerInteractionHandler>();
    }

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check for birds if the timer exceeds the interval
        if (timer >= checkInterval)
        {
            // Reset the timer
            timer = 0f;

            if (playerInteractionHandler != null)
            {
                CheckForBirdsAndPlayer(); //check if there are birds within the specified range
                CheckIfPlayerIsFacingBirds(); //check player is facing the birds

                if (anyBirdsInRange && ekekekTrigger.playerInEkekekZone && isFacingBirds)
                {
                    playerInteractionHandler.Ekekek(true); //the player with ekekek if you are facing the birds AND are within the ekekek zone AND there are birds within range
                }
                else
                {
                    playerInteractionHandler.Ekekek(false);
                }

                if (isPlayerInRange)
                {
                    ScareBirds(); //scare away the birds if the player comes within range
                }
            }
            else
            {
                playerInteractionHandler = FindObjectOfType<PlayerInteractionHandler>();
            }
        }
    }

    //check if there are any birds or the player within range
    private void CheckForBirdsAndPlayer()
    {
        hasFoundBirds = false;
        hasFoundPlayer = false;

        // Perform a non-allocating sphere cast to detect birds within range
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, scareRange, scareCollidersBuffer, scareLayerMask);

        // Check if any detected colliders have the "Bird" or "Player" tag
        for (int i = 0; i < numColliders; i++)
        {
            if (scareCollidersBuffer[i].CompareTag("Bird"))
            {
                hasFoundBirds = true;
            }
            if (scareCollidersBuffer[i].tag == "Player")
            {
                hasFoundPlayer = true;
            }
        }

        anyBirdsInRange = hasFoundBirds;
        isPlayerInRange = hasFoundPlayer;
    }

    // Visualize the detection range in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scareRange);
    }

    private void ScareBirds()
    {
        foreach (var landingspot in landingSpots)
        {
            landingspot.ReleaseFlockChild();
        }
    }

    private void CheckIfPlayerIsFacingBirds()
    {
        if (playerInteractionHandler != null)
        {
            Vector3 directionToTarget = landingSpots[0].transform.position - playerInteractionHandler.gameObject.transform.position;

            // Calculate the dot product of the forward direction of this GameObject and the direction to the target
            float dotProduct = Vector3.Dot(playerInteractionHandler.gameObject.transform.forward, directionToTarget.normalized);

            // Check if the dot product is greater than a threshold value (indicating they are facing in a similar direction)
            if (dotProduct > facingBirdsFov) 
            {
                isFacingBirds = true;
            }
            else
            {
                isFacingBirds = false;
            }
        }
    }
}
