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
using Yarn;
using Yarn.Unity;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class InterfaceManager : MonoBehaviour
{
    //Other script references
    public static InterfaceManager instance;
    public DialogueRunner dialogueRunner;
    public FadeController fader;

    //Script variables
    public CharacterScript currentChar;


    private void Awake()
    {
        instance = this;
    }
    
    /// <summary>
    /// Called through a unity event when yarn spinner finishes dialogue
    /// </summary>
    public void OnDialogueEnd() {
        //Animator tempAnimator;
        //Debug.Log("I am not talking anymore");
        GameManager.instance.dialogueCamera.gameObject.SetActive(false);
        PlayerInteractionHandler.instance.animator.SetBool("talking", false);
        //tempAnimator = currentChar.GetComponentInParent<Animator>();
        //tempAnimator.SetBool("talking", false);
        //tempAnimator = null;
        //PlayerInteractionHandler.instance.animator.SetFloat("MotionSpeed", 0);
        PlayerInteractionHandler.instance.thirdPersonController.enabled = true;
        PlayerInteractionHandler.instance.enabled = true;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    /// <summary>
    /// Call to start yarn spinner dialogue
    /// </summary>
    /// <param name="node">Which node to play</param>
    public void StartDialogue(string node) {
        GameManager.instance.targetGroup.m_Targets[1].target = PlayerInteractionHandler.instance.closestInteractable.transform;
        GameManager.instance.dialogueCamera.gameObject.SetActive(true);

        PlayerInteractionHandler.instance.thirdPersonController.enabled = false;
        PlayerInteractionHandler.instance.enabled = false;
        PlayerInteractionHandler.instance.animator.SetBool("talking", true);
        PlayerInteractionHandler.instance.animator.SetFloat("Speed", 0);

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        dialogueRunner.StartDialogue(node);
    }

    
}
