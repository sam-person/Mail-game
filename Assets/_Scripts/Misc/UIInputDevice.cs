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

    public OnInputDeviceChanged_Image[] onInputDeviceChanged_Images;
    public InterfaceManager interfaceManager;

    public GameObject lastSelectedGameObject;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        currentDevice = playerInput.currentControlScheme;
        interfaceManager.currentInputDevice = currentDevice;
    }

    void Update()
    {
        //if(EventSystem.current.currentSelectedGameObject != lastSelectedGameObject && EventSystem.current.currentSelectedGameObject != null)
        //{
        //    lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        //}

        //if the input device changes
        if (playerInput.currentControlScheme != currentDevice)
        {
            interfaceManager.currentInputDevice = currentDevice;
            Debug.Log("Change control scheme to " + currentDevice);
            //if the input device is not a Keyboard and Mouse
            if (playerInput.currentControlScheme != "KeyboardMouse")
            {
                usingKBM = false;
                //EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
                interfaceManager.OnInputDeviceChanged("PS");
                foreach (var item in onInputDeviceChanged_Images)
                {
                    item.OnInputDeviceChanged("PS");
                }
            }
            else
            {
                usingKBM = true;
                interfaceManager.OnInputDeviceChanged("KBM");
                EventSystem.current.SetSelectedGameObject(null); //deselect any buttons that are currently selected
                foreach (var item in onInputDeviceChanged_Images)
                {
                    item.OnInputDeviceChanged("KBM");
                }
            }
        }

        currentDevice = playerInput.currentControlScheme;
    }
}
