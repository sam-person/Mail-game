using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_Dialogue : Receiver
{
    public string YarnNode = "";
    public Transform cameraTarget;

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.StartDialogue(YarnNode, cameraTarget);
    }
}
