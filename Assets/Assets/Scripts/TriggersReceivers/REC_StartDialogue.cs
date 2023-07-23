using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_StartDialogue : Receiver

{


    Coroutine talkingCoroutine;

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.focusTarget = this.gameObject;
        InterfaceManager.instance.currentChar = this.GetComponent<CharacterScript>();
        talkingCoroutine = StartCoroutine(InterfaceManager.instance.DialogueController(true));

    }
}
