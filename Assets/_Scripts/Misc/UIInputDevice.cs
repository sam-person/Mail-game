using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputDevice : MonoBehaviour
{
    PlayerInput playerInput;
    private string currentDevice;

    public OnInputDeviceChanged_Image[] onInputDeviceChanged_Images;
    public InterfaceManager interfaceManager;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        //if the input device changes
        if (playerInput.currentControlScheme != currentDevice)
        {
            Debug.Log("Change control scheme to " + currentDevice);
            //if the input device is a Keyboard and Mouse
            if (playerInput.currentControlScheme != "KeyboardMouse")
            {
                interfaceManager.OnInputDeviceChanged("PS");
                //foreach (var item in onInputDeviceChanged_Images)
                //{
                //    item.OnInputDeviceChanged("PS");
                //}
            }
            else
            {
                interfaceManager.OnInputDeviceChanged("KBM");
                //foreach (var item in onInputDeviceChanged_Images)
                //{
                //    item.OnInputDeviceChanged("KBM");
                //}
            }
        }

        currentDevice = playerInput.currentControlScheme;
    }
}
