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
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

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
    public GameObject pauseMenuControls;
    public GameObject debugObject;

    [BoxGroup("Dialogue")]
    public TextMeshProUGUI dialogueCharacterName, dialogueText, dialogueNextText;
    [BoxGroup("Dialogue")]
    public Image dialogueBackgroundImage, dialogueOutlineImage, dialogueLeftWhiskers, dialogueRightWhiskers;
    [BoxGroup("Dialogue/Options")]
    public TextMeshProUGUI /*dialogue_options_CharacterName,*/ dialogue_options_Text;
    [BoxGroup("Dialogue/Options")]
    public Image dialogue_options_BackgroundImage, dialogue_options_OutlineImage, dialogue_options_LeftWhiskers, dialogue_options_RightWhiskers;
    public Transform continueButtonMousePos;

    [BoxGroup("TutorialPanels")]
    public Transform tutorialParent;
    [BoxGroup("TutorialPanels")]
    public UI_Tutorial_Panel panelObject;

    public RectTransform interactIcon;
    public Image interactImage;

    public TextMeshProUGUI questText;

    public Sprite interact_active_icon, interact_locked_icon, interact_active_icon_PS, interact_active_icon_XB, interact_active_icon_KBM;

    public Canvas UIcanvas;

    public string currentInputDevice;
    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void ShowDebug()
    {
        debugGameState.text = GameManager.instance.gameState.ToString();
        debugObject.SetActive(!debugObject.activeInHierarchy);
    }
    private void Update()
    {
        if (PlayerInteractionHandler.instance.closestInteractable != null && GameManager.instance.gameState == GameManager.GameState.Gameplay)
        {
            interactIcon.gameObject.SetActive(true);
            Vector3 iconPos = PlayerInteractionHandler.instance.closestInteractable.interactionPivotOverride == null ? PlayerInteractionHandler.instance.closestInteractable.transform.position : PlayerInteractionHandler.instance.closestInteractable.interactionPivotOverride.position;
            interactIcon.anchoredPosition = Camera.main.WorldToScreenPoint(iconPos) * (1f/UIcanvas.scaleFactor);

            switch (PlayerInteractionHandler.instance.closestInteractable.interactionState)
            {
                case TRI_Interactable.InteractionType.Active:
                default:
                    interactImage.sprite = interact_active_icon;
                    break;
                case TRI_Interactable.InteractionType.Inactive:
                    //this shouldn't be possible...
                    break;
                case TRI_Interactable.InteractionType.Locked:
                    interactImage.sprite = interact_locked_icon;
                    break;
                
            }
        }
        else {
            interactIcon.gameObject.SetActive(false);
        }
    }

    public void ShowPauseMenu(bool _active) {
        pauseMenu.SetActive(_active);
        pauseMenu.GetComponent<CanvasGroup>().interactable = _active;
    }

    public void PauseButton_Quit() {

        //The below lines close the game
        //Application.Quit();
        //GameManager.instance.Quit();

        //Get a reference to the yarnstorage script and use it to clear all yarn variables
        Yarn.Unity.InMemoryVariableStorage yarnStorage = FindObjectOfType<Yarn.Unity.InMemoryVariableStorage>();
        yarnStorage.Clear();

        //Unpause the game, destroy this object, and load the main menu scene
        GameManager.instance.TogglePause();
        Destroy(this.gameObject);
        SceneManager.LoadScene("MainMenu");

    }

    public void PauseButton_Resume()
    {
        GameManager.instance.TogglePause();
        pauseMenu.GetComponent<CanvasGroup>().interactable = false;
    }

    public void PauseMenu_Controls(bool _active)
    {
        pauseMenuControls.SetActive(_active);
        ShowPauseMenu(!_active);
    }

    /// <summary>
    /// Called through a unity event when yarn spinner finishes dialogue
    /// </summary>
    public void OnDialogueEnd() {
        GameManager.instance.SetGameState(GameManager.GameState.Gameplay);
        GameManager.instance.currentNPC.OnDialogueEnd();
        GameManager.instance.currentNPC = null;
        GameManager.instance._onDynamicYarnVariableChange.Invoke();
        if (dialogueCamera != null) dialogueCamera.gameObject.SetActive(false);
    }

    public void OnDialogueLine() {
        GameManager.instance.currentNPC.OnDialogueLine();
    }

    CinemachineVirtualCamera dialogueCamera;

    /// <summary>
    /// Call to start yarn spinner dialogue
    /// </summary>
    /// <param name="node">Which node to play</param>
    public void StartDialogue(string node, CinemachineVirtualCamera _dialogueCamera, NPCDefinition npc = null) {
        if (npc != null) {
            //main line
            dialogueText.font = npc.font;
            //dialogueCharacterName.font = npc.font;
            dialogueText.color = npc.fontColour;
            dialogueCharacterName.color = npc.nameColour;
            dialogueNextText.color = npc.backgroundColor;

            dialogueBackgroundImage.color = npc.backgroundColor;
            dialogueOutlineImage.color = npc.outlineColor;
            dialogueLeftWhiskers.color = npc.leftWhiskerColor;
            dialogueRightWhiskers.color = npc.rightWhiskerColor;

            //options
            dialogue_options_Text.font = npc.font;
            dialogue_options_Text.color = npc.fontColour;
            dialogue_options_BackgroundImage.color = npc.backgroundColor;
            dialogue_options_OutlineImage.color = npc.outlineColor;
            dialogue_options_LeftWhiskers.color = npc.leftWhiskerColor;
            dialogue_options_RightWhiskers.color = npc.rightWhiskerColor;
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

    public void SpawnTutorialPanel(Sprite _image, Sprite _imagePS, string _text, string _textPS, float _time) 
    {
        UI_Tutorial_Panel spawned = Instantiate(panelObject, tutorialParent);
        spawned.gameObject.SetActive(true);

        if (currentInputDevice == "PS")
        {
            spawned.SetPanel(_imagePS, _textPS, _time);
        }
        else if (currentInputDevice == "KBM")
        {
            spawned.SetPanel(_image, _text, _time);
        }
        else
        {
            spawned.SetPanel(_image, _text, _time);
        }
    }

    public void OnInputDeviceChanged(string device)
    {
        if (device == "PS")
        {
            GameManager.instance.isUsingKBM = false;
            interact_active_icon = interact_active_icon_PS;
        }
        else if (device == "XB")
        {
            GameManager.instance.isUsingKBM = false;
            interact_active_icon = interact_active_icon_XB;
        }
        else
        {
            GameManager.instance.isUsingKBM = true;
            interact_active_icon = interact_active_icon_KBM;
        }
    }

    public void SetMousePosToBottomRight()
    {
        Debug.Log("Setting mouse position to bottom right of screen");
        Vector2 bottomRightPosition = new Vector2(continueButtonMousePos.position.x, continueButtonMousePos.position.y);
        //Cursor.SetCursor(null, bottomRightPosition, CursorMode.Auto);
        Mouse.current.WarpCursorPosition(bottomRightPosition);
    }

    public void UpdateNPCInfo(NPCDefinition npc)
    {
        //main line
        dialogueText.font = npc.font;
        //dialogueCharacterName.font = npc.font;
        dialogueText.color = npc.fontColour;
        dialogueCharacterName.color = npc.nameColour;
        dialogueNextText.color = npc.backgroundColor;

        dialogueBackgroundImage.color = npc.backgroundColor;
        dialogueOutlineImage.color = npc.outlineColor;
        dialogueLeftWhiskers.color = npc.leftWhiskerColor;
        dialogueRightWhiskers.color = npc.rightWhiskerColor;

        //options
        dialogue_options_Text.font = npc.font;
        dialogue_options_Text.color = npc.fontColour;
        dialogue_options_BackgroundImage.color = npc.backgroundColor;
        dialogue_options_OutlineImage.color = npc.outlineColor;
        dialogue_options_LeftWhiskers.color = npc.leftWhiskerColor;
        dialogue_options_RightWhiskers.color = npc.rightWhiskerColor;
    }
}

