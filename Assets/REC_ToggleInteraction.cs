using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_ToggleInteraction : Receiver
{
    public TRI_Interactable interactable;

    public enum ToggleType { Toggle, Enable, Disable };
    [SerializeField] private ToggleType toggleType;

    public override void Activate()
    {
        base.Activate();
        switch (toggleType)
        {
            case ToggleType.Toggle:
                interactable.enabled = !interactable.enabled;
                break;
            case ToggleType.Enable:
                interactable.enabled = true;
                break;
            case ToggleType.Disable:
                interactable.enabled = false;
                break;
        }
        
    }
}
