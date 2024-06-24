using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UIInputDevice : MonoBehaviour
{
    PlayerInput playerInput;
    private string currentDevice;
    public bool usingKBM = true;

    public bool autoSelectControlScheme = true;
    public enum controlScheme { KBM, PS, XB}
    public controlScheme currentControlScheme = controlScheme.KBM;

    public controlScheme overridenControlScheme = controlScheme.KBM;
    public bool overrideControlScheme = false;

    public OnInputDeviceChanged_Image[] onInputDeviceChanged_Images;
    public InterfaceManager interfaceManager;

    public GameObject lastSelectedGameObject;
    public bool isMainMenu = false;
    public GameObject mainMenuButton;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        //if(EventSystem.current.currentSelectedGameObject != lastSelectedGameObject && EventSystem.current.currentSelectedGameObject != null)
        //{
        //    lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        //}

        //if the input device changes
        if (playerInput.currentControlScheme != currentDevice && autoSelectControlScheme)
        {
            Debug.Log("Change control scheme to " + playerInput.currentControlScheme);
            //if the input device is a Keyboard and Mouse
            if (playerInput.currentControlScheme == "KeyboardMouse")
            {
                SelectedControlScheme(controlScheme.KBM);
            }
            else if (playerInput.currentControlScheme == "Gamepad")
            {
                SelectedControlScheme(controlScheme.PS);
            }
        }
        currentDevice = playerInput.currentControlScheme;
    }

    public void SelectedControlScheme(controlScheme control)
    {
        switch (control)
        {
            case controlScheme.KBM:
                HandleChangeInput(controlScheme.KBM, "KBM");
                break;
            case controlScheme.PS:
                HandleChangeInput(controlScheme.PS, "PS");
                break;
            case controlScheme.XB:
                HandleChangeInput(controlScheme.XB, "XB");
                break;
            default:
                HandleChangeInput(controlScheme.KBM, "KBM");
                break;
        }
    }


    void HandleChangeInput(controlScheme control, string controlString)
    {
        currentControlScheme = control;
        if(interfaceManager != null)
        { 
            interfaceManager.currentInputDevice = controlString; 
        }
        

        if(controlString == "KBM")
        {
            usingKBM = true;
            
            if(isMainMenu)
            {
                lastSelectedGameObject = mainMenuButton;
            }
            else
            {
                lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            }
            EventSystem.current.SetSelectedGameObject(null); //deselect any buttons that are currently selected
        }
        else
        {
            usingKBM = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if(lastSelectedGameObject != null && isMainMenu)
            {
                EventSystem.current.SetSelectedGameObject(mainMenuButton);
            }
        }

        if (interfaceManager != null)
        {
            interfaceManager.OnInputDeviceChanged(controlString);
        }

        if (onInputDeviceChanged_Images != null)
        {
            foreach (var item in onInputDeviceChanged_Images)
            {
                item.OnInputDeviceChanged(controlString);
            }
        }
    }

    public void OverrideControlScheme(controlScheme control)
    {
        overridenControlScheme = control;
        overrideControlScheme = true;
        SelectedControlScheme(control);
    }
}
