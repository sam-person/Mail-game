using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenIntefaction : MonoBehaviour
{
    public Animator ovenAnimator;
    public GameObject particles;
    public GameManager gameManager;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Previous Gamestate was " + gameManager.gameState);
            ovenAnimator.SetBool("isTriggered", true);
            particles.SetActive(false);
            gameManager.gameState = 2;
            Debug.Log("New Gamestate is " + gameManager.gameState);

        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            particles.SetActive(true);
        }
    }
}
