using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static REC_NPC.NPC_DialogueNode;

public class TRI_Interactable : Trigger
{
    public float interactionRange = 2f;

    

    public cakeslice.Outline outline;

    private void OnEnable()
    {
        GameManager.instance.interactables.Add(this);
    }

    private void OnDisable()
    {
        GameManager.instance.interactables.Remove(this);
    }

    public override void Activate()
    {
        base.Activate();
    }
}
