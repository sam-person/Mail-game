using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_SetInteractableState : Receiver
{
    public TRI_Interactable interactable;
    public TRI_Interactable.InteractionType interactionState;

    public override void Activate()
    {
        base.Activate();
        interactable.SetInteractionState(interactionState);
    }
}
