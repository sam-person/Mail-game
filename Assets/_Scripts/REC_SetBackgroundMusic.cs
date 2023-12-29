using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class REC_SetBackgroundMusic : Receiver
{
    [InfoBox("Changes the audio back to the base background music (whatever was in the BGM audio at the start of the scene)", "changeToBaseAudio")]
    public bool changeToBaseAudio = false;
    [HideIf("changeToBaseAudio")]
    public AudioClip clipToChangeTo;

    public override void Activate()
    {
        base.Activate();
        if (changeToBaseAudio) {
            GameManager.instance.SwitchBGMAudioToBase();

        }
        else {
            GameManager.instance.SwitchBGMAudio(clipToChangeTo);
        }
    }
}
