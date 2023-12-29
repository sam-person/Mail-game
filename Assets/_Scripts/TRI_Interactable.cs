using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static REC_NPC.NPC_DialogueNode;

[RequireComponent(typeof(Outline))]
public class TRI_Interactable : Trigger
{
    public enum InteractionType {Active, Inactive, Locked }; //add more types like door, npc, etc
    public InteractionType interactionState = InteractionType.Active;

    public float interactionRange = 2f;

    public Transform interactionPivotOverride;

    private void Reset()
    {
        outline = GetComponent<Outline>();
    }

    public cakeslice.Outline outline;

    private void OnEnable()
    {
        GameManager.instance.interactables.Add(this);
        SetInteractionColour();
    }

    public void SetInteractionState(InteractionType state) {
        interactionState = state;
        SetInteractionColour();
    }

    void SetInteractionColour()
    {
        switch (interactionState)
        {
            case InteractionType.Active:
            default:
                outline.color = 0;
                break;
            case InteractionType.Inactive:
                outline.color = 0;
                break;
            case InteractionType.Locked:
                outline.color = 1;
                break;

        }

    }

    private void OnDisable()
    {
        GameManager.instance.interactables.Remove(this);
    }

    bool GetCanActivate() { 
        return interactionState == InteractionType.Active; //with more interaction stsates, expand this check with a bunch of OR ||
    }

    public override void Activate()
    {
        if(GetCanActivate()) base.Activate();
    }

}
