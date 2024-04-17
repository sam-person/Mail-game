using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Cinemachine;
using TMPro;
using Yarn.Unity;
using Yarn;
using System;

public class GameManager : MonoBehaviour
{

    public AudioSource bgmAudio;


    //Cinemachine References
    //public CinemachineTargetGroup targetGroup;
    //public CinemachineVirtualCamera dialogueCamera;
    public CinemachineVirtualCamera playerFollowCamera;
    public Camera mainCamera;

    public enum GameState {Gameplay, Dialogue, Paused };
    public GameState gameState = GameState.Gameplay;
    public GameState previousState;

    [ReadOnly]
    public Transform cameraTarget;

    public Subarea currentSubarea;

    public REC_NPC currentNPC;

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

    InMemoryVariableStorage yarnVariables;

    AudioClip _startAudio;

    // Start is called before the first frame update
    void Start()
    {
        yarnVariables = InterfaceManager.instance.GetComponent<InMemoryVariableStorage>();
        SetGameState(GameState.Gameplay);
        _startAudio = bgmAudio.clip;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseInput();
    }
    public void SetGameState(GameState newState) {
        //check that we're changing to a new state
        //if (gameState == newState) return;

        //keep track of the old state
        previousState = gameState;
        //leave the old state
        OnStateExit(gameState);
        //enter the newState
        OnStateEnter(newState);
        //set the new state
        gameState = newState;

        _onGameStateChange?.Invoke();
    }

    void OnStateExit(GameState oldState) {
        switch (oldState)
        {
            case GameState.Gameplay:
                break;
            case GameState.Dialogue:
                PlayerInteractionHandler.instance.animator.SetBool("talking", false);
                //dialogueCamera.gameObject.SetActive(false);
                //targetGroup.m_Targets[1].target = null;
                break;
            case GameState.Paused:
                Time.timeScale = 1f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                InterfaceManager.instance.ShowPauseMenu(false);

                break;
        }
    }

    void OnStateEnter(GameState newState)
    {
        switch (newState)
        {
            case GameState.Gameplay:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                PlayerInteractionHandler.instance.thirdPersonController.enabled = true;
                PlayerInteractionHandler.instance.enabled = true;
                PlayerInteractionHandler.instance.StartInteractionCooldown();//whenever we enter gameplay, start an interaction cooldown
                break;
            case GameState.Dialogue:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                PlayerInteractionHandler.instance.thirdPersonController.enabled = false;
                PlayerInteractionHandler.instance.enabled = false;
                PlayerInteractionHandler.instance.animator.SetBool("talking", true);
                PlayerInteractionHandler.instance.animator.SetFloat("Speed", 0);
                //disable all the outlines
                foreach (TRI_Interactable interactable in interactables) {
                    if(interactable.outline) interactable.outline.enabled = false;
                }
                //if (cameraTarget != null)
                //{
                    //SetDialogueCameraTarget(1,cameraTarget);

                //}
                //else {
                //    SetDialogueCameraTarget(1, PlayerInteractionHandler.instance.thirdPersonController.CinemachineCameraTarget.transform);
                //}
                //dialogueCamera.gameObject.SetActive(true);
                break;
            case GameState.Paused:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                InterfaceManager.instance.ShowPauseMenu(true);
                break;
        }

        
    }

    //public void SetDialogueCameraTarget(int num, Transform target) {
    //    //targetGroup.m_Targets[num].target = target;
    //}

    void HandlePauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        switch (gameState)
        {
            case GameState.Gameplay:
                SetGameState(GameState.Paused);
                break;
            case GameState.Dialogue:
                SetGameState(GameState.Paused);
                break;
            case GameState.Paused:
                switch (previousState)
                {
                    case GameState.Gameplay:
                        SetGameState(GameState.Gameplay);
                        break;
                    case GameState.Dialogue:
                        SetGameState(GameState.Dialogue);
                        break;
                    case GameState.Paused:
                        SetGameState(GameState.Gameplay);
                        break;
                }
                break;
        }
    }


    public void Quit() 
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        }
#endif

        Application.Quit();
    }



    public T GetYarnVariable<T>(string variableName) {
        T output;

        if (yarnVariables.TryGetValue(variableName, out output))
        {
            return output;
        }
        else {
            return default(T);
        }
    }

    public void SetYarnVariable(string variableName, string value, REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType VariableType) {
        switch (VariableType)
        {
            case REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.String:
                yarnVariables.SetValue(variableName, value);
                break;
            case REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.Bool:
                yarnVariables.SetValue(variableName, bool.Parse(value));
                break;
            case REC_NPC.NPC_DialogueNode.NPC_DialogueCondition.VariableType.Int:
                yarnVariables.SetValue(variableName, int.Parse(value));
                break;
        }
        //yarnVariables.SetValue(variableName, value);
    }

    public REC_Teleport pendingTeleport;
    public REC_SceneChange pendingSceneChange;

    public void StartTeleport(REC_Teleport teleporter) {
        if (pendingTeleport != null) {
            Debug.Log("Can't start this teleport because there is already one pending!");
            return;
        }

        pendingTeleport = teleporter;
        InterfaceManager.instance.fader.Fade();
    }

    public void StartSceneChange(REC_SceneChange scenechanger)
    {
        if (pendingSceneChange != null)
        {
            Debug.Log("Can't start this scenechange because there is already one pending!");
            return;
        }

        pendingSceneChange = scenechanger;
        pendingTeleport = null; // clear any pending teleport
        InterfaceManager.instance.fader.Fade();
    }

    public void OnMidFade() {
        if (pendingTeleport != null) {
            pendingTeleport.Teleport();
            pendingTeleport = null;
            return;
        }

        if (pendingSceneChange != null)
        {
            pendingSceneChange.ChangeScene();
            pendingSceneChange = null;
        }
    }

    /// <summary>
    /// When dialogue ends, when the setyarnvariable is triggered
    /// </summary>
    /// 
    public delegate void OnDynamicYarnVariableChange();
    public OnDynamicYarnVariableChange _onDynamicYarnVariableChange;

    public delegate void OnGameStateChange();
    public OnGameStateChange _onGameStateChange;

    /// <summary>
    /// Change the bgm audio to a certain clip
    /// </summary>
    /// <param name="audio"></param>
    public void SwitchBGMAudio(AudioClip audio) {
        bgmAudio.clip = audio;
        bgmAudio.Stop();
        bgmAudio.Play();
    }

    /// <summary>
    /// Change back to the base audio (what ever was in the audioplayer at the start)
    /// </summary>
    public void SwitchBGMAudioToBase()
    {
        bgmAudio.clip = _startAudio;
        bgmAudio.Stop();
        bgmAudio.Play();
    }

}
