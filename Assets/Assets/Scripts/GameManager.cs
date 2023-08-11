using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionHandler;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int gameStateInt;
    public bool isGamePaused = false;
    public AudioSource bgmAudio;

    public GameObject UIButtonGroup;



    //Delegate declaration and instancing
    public delegate void GamePaused(bool isPaused);
    public static event GamePaused gamePaused;

    public static GameManager Instance;
    public TimeOfDay TimePeriod;




    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("The game state is " + gameStateInt);
        gamePaused += TurnOffAnimators;
        gamePaused += PauseAudio;
        gamePaused += ShowUIButtons;
        gamePaused += FreezeTime;
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

    public void Button_Exit()
    {

        Application.Quit();
        EditorApplication.isPlaying = false;

    }

    public void Button_Unpause()
    {
        gamePaused?.Invoke(false);
        isGamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }



public void ShowUIButtons(bool isPaused)
    {
        if (isPaused)
        {
            UIButtonGroup.SetActive(true);
        }
        else
        {
            UIButtonGroup.SetActive(false);
        }
    }

    public void FreezeTime(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
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
                Cursor.lockState = CursorLockMode.Confined;
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePaused?.Invoke(false);
                isGamePaused = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }




    public void UpdateTimeOfDay(TimeOfDay newTime)
    {
        TimePeriod = newTime;
        gameStateInt = 0;
        switch (newTime)
        {

            case TimeOfDay.EarlyMorning:
                break;
                
            case TimeOfDay.MidMorning:
 
                break;
                
            case TimeOfDay.Lunchtime:
                break;
                
            case TimeOfDay.Afternoon:
                break;

            case TimeOfDay.Evening:
                break;
        }
    }

    public enum TimeOfDay
    {
        EarlyMorning,
        MidMorning,
        Lunchtime,
        Afternoon,
        Evening,
    }

}
