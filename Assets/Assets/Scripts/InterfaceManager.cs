using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using System;
using TMPro.Examples;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Sirenix.Utilities;

public class InterfaceManager : MonoBehaviour
{
    //Other script references
    public static InterfaceManager instance;
    public PlayerInteractionHandler playerInteractionHandler;
    public GameManager gameManager;

    //Cinemachine References
    public CinemachineTargetGroup targetGroup;
    public CinemachineVirtualCamera dialogueCamera;

    //GameObject References
    public GameObject DialogueUI;
    public Image nameBubble;
    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI dialogueText;
    public TeleType teleType;


    //Script variables
    public CharacterScript currentChar;
    public string currentDialogue;
    public int conversationStage = 0;
    public bool dialogueEnabled;
    bool firstTime = false;
    public GameObject focusTarget;

    public float timer = 0.5f;
    public bool haveWaited = false;
    public bool updateTimerCalled = false;

    public List<string> dialogueArray = new List<string>();

    //public List<DialogueData>[] dialogueArray;


    //Coroutines
    Coroutine teletypingCoroutine;
    Coroutine talkingCoroutine;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCharacterInfo()
    {
        nameTMP.text = currentChar.data.CharacterName;
        nameTMP.color = currentChar.data.CharacterNameColor;
        nameBubble.color = currentChar.data.CharacterColor;
    }

    public void StartTalking()
    {

        if (gameManager.gameState == 1)
        {
            if (!firstTime)
            {
                foreach (string conversation in currentChar.DialogueList[0].conversationBlock)
                {
                    dialogueArray.Add(conversation);

                    if (dialogueArray.Count == currentChar.DialogueList[0].conversationBlock.Count)
                    {
                        firstTime = true;
                    }

                }
            }
            if (conversationStage < dialogueArray.Count)
            {
             
                currentDialogue = currentChar.DialogueList[0].conversationBlock[conversationStage];
                dialogueText.text = currentDialogue;
                teletypingCoroutine = StartCoroutine(teleType.TypeText());
            }
            else
            {
                firstTime = false;
                conversationStage = 0;
                talkingCoroutine = StartCoroutine(DialogueController(false));
                //DialogueController(false);
                dialogueEnabled = false;
                dialogueArray.Clear();

            }

        }


    }

    public IEnumerator DialogueController(bool currentlytalking)
    {
        Animator tempAnimator;
        if (currentChar != null)
        {
            Debug.Log("Current Character is " + currentChar);

        }
        else
        {
            Debug.Log("Current character is NULL");
        }

        if (currentlytalking && currentChar != null)
        {

            haveWaited = false;
            updateTimerCalled = false;

            Debug.Log("I am now talking");
            dialogueEnabled = true;
            //targetGroup.m_Targets[1].target = playerInteractionHandler.focusCollider.gameObject.transform;
            targetGroup.m_Targets[1].target = focusTarget.transform;

            dialogueCamera.gameObject.SetActive(true);

            playerInteractionHandler.thirdPersonController.enabled = false;
            playerInteractionHandler.enabled = false;
            playerInteractionHandler.animator.SetBool("talking", true);
            playerInteractionHandler.animator.SetFloat("Speed", 0);

            if (currentChar.TryGetComponent<Animator>(out Animator animator))
            {
                currentChar.animator.SetBool("talking", true);

            }
            else
            {
                tempAnimator = currentChar.GetComponentInParent<Animator>();
                tempAnimator.SetBool("talking", true);
            }

            
            UpdateCharacterInfo();
            DialogueUI.SetActive(true);
            StartTalking();


            yield return new WaitForEndOfFrame();
        }

        else if (currentChar == null)
        {
            yield break;
        }
        else
        {
            Debug.Log("I am not talking anymore");
            dialogueCamera.gameObject.SetActive(false);
            DialogueUI.SetActive(false);
            dialogueEnabled = false;
            playerInteractionHandler.animator.SetBool("talking", false);
            tempAnimator = currentChar.GetComponentInParent<Animator>();
            tempAnimator.SetBool("talking", false);
            tempAnimator = null;
            //playerInteractionHandler.animator.SetFloat("MotionSpeed", 0);
            yield return new WaitForSeconds(0.75f);
            playerInteractionHandler.thirdPersonController.enabled = true;
            playerInteractionHandler.enabled = true;


            yield return new WaitForEndOfFrame();



        }
    }
}
