using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int gameState;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("The game state is " + gameState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            gameState = 1;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            gameState = 2;
        }
    }
}
