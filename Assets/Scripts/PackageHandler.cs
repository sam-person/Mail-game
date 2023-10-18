using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageHandler : MonoBehaviour, IInteractable
{
    public GameManager gameManager;
    public bool play;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GetComponent<GameManager>();
    }

    private void PickUpPackage(bool activate)
    {
        if (activate)
        {
            gameManager.gameState += 1;
            Debug.Log(this.name + "increased game state by 1");
            Debug.Log("Current game state is " + gameManager.gameState);
            this.gameObject.SetActive(false);
        }

        else
        {
            this.gameObject.SetActive(true);
        }
        
    }


    public void Interact(bool activate)
    {
        PickUpPackage(true);
    }

}
