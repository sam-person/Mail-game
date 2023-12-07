using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_SetQuest : Receiver
{
    public string questText = "Quest Text";

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.SetQuestText(questText);
    }
}
