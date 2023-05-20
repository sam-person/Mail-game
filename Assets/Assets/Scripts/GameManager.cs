using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionHandler;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int gameState;

    public bool isGamePaused = false;

    public AudioSource bgmAudio;


    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;



    //Delegate declaration and instancing
    public delegate void GamePaused(bool isPaused);
    public static event GamePaused gamePaused;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("The game state is " + gameState);
        gamePaused += TurnOffAnimators;
        gamePaused += PauseAudio;
        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void TurnOffAnimators(bool animsOff)
    {
        Animator[] animators = (Animator[]) GameObject.FindObjectsOfType(typeof(Animator));

        foreach (Animator animator in animators)
        {
            animator.enabled = !animsOff;
        }
    }


    public void PauseAudio(bool isPaused)
    {
        float startingBGMAudio = bgmAudio.volume;

        if (isPaused)
        {
            bgmAudio.volume = startingBGMAudio / 3;
        }
        else
        {
            bgmAudio.volume = startingBGMAudio * 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePaused?.Invoke(true);
                isGamePaused = true;

                
                Cursor.visible = true;
                
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePaused?.Invoke(false);
                isGamePaused = false;
                Cursor.visible = false;
            }
        }
    }
}
