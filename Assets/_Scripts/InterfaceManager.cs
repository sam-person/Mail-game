using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using System;
using Yarn;
using Yarn.Unity;

public class InterfaceManager : MonoBehaviour
{
    //Other script references
    public static InterfaceManager instance;
    public DialogueRunner dialogueRunner;
    public FadeController fader;

    //Script variables
    public CharacterScript currentChar;

    public TextMeshProUGUI debugGameState;

    public GameObject pauseMenu;
    public GameObject debugObject;

    public Image dialogueBackgroundImage, dialogueOutlineImage, dialogueLeftWhiskers, dialogueRightWhiskers;
    public TextMeshProUGUI dialogueCharacterName, dialogueText;

    public RectTransform interactIcon;

    public TextMeshProUGUI questText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        debugGameState.text = GameManager.instance.gameState.ToString();
        if (Input.GetKeyDown(KeyCode.F)) {
            debugObject.SetActive(!debugObject.activeInHierarchy);
        }

        if (PlayerInteractionHandler.instance.closestInteractable != null && GameManager.instance.gameState == GameManager.GameState.Gameplay)
        {
            interactIcon.gameObject.SetActive(true);
            interactIcon.anchoredPosition = Camera.main.WorldToScreenPoint(PlayerInteractionHandler.instance.closestInteractable.transform.position);
        }
        else {
            interactIcon.gameObject.SetActive(false);
        }
    }

    public void ShowPauseMenu(bool _active) {
        pauseMenu.SetActive(_active);
    }

    public void PauseButton_Quit() {
        GameManager.instance.Quit();
    }

    public void PauseButton_Resume()
    {
        GameManager.instance.TogglePause();
    }

    /// <summary>
    /// Called through a unity event when yarn spinner finishes dialogue
    /// </summary>
    public void OnDialogueEnd() {
        GameManager.instance.SetGameState(GameManager.GameState.Gameplay);
        if (dialogueCamera != null) dialogueCamera.gameObject.SetActive(false);
    }

    CinemachineVirtualCamera dialogueCamera;

    /// <summary>
    /// Call to start yarn spinner dialogue
    /// </summary>
    /// <param name="node">Which node to play</param>
    public void StartDialogue(string node, CinemachineVirtualCamera _dialogueCamera, NPCDefinition npc = null) {
        if (npc != null) {
            dialogueText.font = npc.font;
            dialogueCharacterName.font = npc.font;
            dialogueText.color = npc.fontColour;
            dialogueCharacterName.color = npc.fontColour;

            dialogueBackgroundImage.color = npc.backgroundColor;
            dialogueOutlineImage.color = npc.outlineColor;
            dialogueLeftWhiskers.color = npc.leftWhiskerColor;
            dialogueRightWhiskers.color = npc.rightWhiskerColor;
        }

        dialogueCamera = _dialogueCamera;
        if(dialogueCamera != null) dialogueCamera.gameObject.SetActive(true);
        GameManager.instance.SetGameState(GameManager.GameState.Dialogue);
        dialogueRunner.StartDialogue(node);
    }

    [YarnCommand("setQuest")]
    public static void SetQuestText(string text) {
        instance.questText.text = text;
    }
    
}
