using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteractionHandler;
using UnityEditor;
using Sirenix.OdinInspector;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int gameState;
    public bool isGamePaused = false;
    public AudioSource bgmAudio;

    public GameObject UIButtonGroup;

    //Cinemachine References
    public CinemachineTargetGroup targetGroup;
    public CinemachineVirtualCamera dialogueCamera;
    public CinemachineVirtualCamera playerFollowCamera;
    public Camera mainCamera;



    //Delegate declaration and instancing
    public delegate void GamePaused(bool isPaused);
    public static event GamePaused gamePaused;


    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }
    #endregion

    //Store a list of all interactables
    [ReadOnly]
    public List<TRI_Interactable> interactables;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("The game state is " + gameState);
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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

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
}
