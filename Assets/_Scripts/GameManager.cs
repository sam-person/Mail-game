using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Cinemachine;
using TMPro;
using Yarn.Unity;
using Yarn;

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

    // Start is called before the first frame update
    void Start()
    {
        yarnVariables = InterfaceManager.instance.GetComponent<InMemoryVariableStorage>();
        SetGameState(GameState.Gameplay);
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

    public void Quit() {
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



}
