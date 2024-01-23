using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Obsolete("This is a deprecated component. Its functionality has been disabled.")]
public class REC_Dialogue : Receiver
{
    public string YarnNode = "";
    CinemachineVirtualCamera dialogueCamera;
    public NPCDefinition NPCDefinition;

    public override void Activate()
    {
        base.Activate();
        //InterfaceManager.instance.StartDialogue(YarnNode, dialogueCamera, NPCDefinition);
        //disabled due to deprecation
    }
}
