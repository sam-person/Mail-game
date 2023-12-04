using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_Dialogue : Receiver
{
    public string YarnNode = "";
    CinemachineVirtualCamera dialogueCamera;
    public NPCDefinition NPCDefinition;

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.StartDialogue(YarnNode, dialogueCamera, NPCDefinition);
    }
}
