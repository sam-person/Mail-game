using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class REC_Teleport : Receiver
{
    public bool inside = true;
    public bool instant = false;
    public Transform spawnPoint;
    public float xRot;
    public float yRot = 10f;

    public enum TeleportType {Enter, Exit, None };
    public TeleportType teleType = TeleportType.Exit;
    [HideIf("teleType", TeleportType.None)]
    public Subarea subarea;

    [BoxGroup("Audio")]
    public bool changeAudio = false;
    [BoxGroup("Audio"), InfoBox("Changes the audio back to the base background music (whatever was in the BGM audio at the start of the scene)", "changeAudioToBase"), ShowIf("changeAudio"), LabelWidth(200f)]
    public bool changeAudioToBase = true;
    [BoxGroup("Audio"), ShowIf("changeAudio"), HideIf("changeAudioToBase")]
    public AudioClip clipToChangeTo;

    public override void Activate()
    {
        base.Activate();
        if (instant)
        {
            Teleport();
        }
        else { 
            GameManager.instance.StartTeleport(this);
        }
    }

    public void Teleport() {
        PlayerInteractionHandler.instance.Teleport(this);
        switch (teleType)
        {
            case TeleportType.Enter:
                GameManager.instance.currentSubarea = subarea;
                subarea.OnEnterSubarea();
                break;
            case TeleportType.Exit:
                GameManager.instance.currentSubarea = null;
                subarea.OnExitSubarea();
                break;
            case TeleportType.None:
                break;
        }
        if (changeAudio) {
            if (changeAudioToBase)
            {
                GameManager.instance.SwitchBGMAudioToBase();
            }
            else {
                GameManager.instance.SwitchBGMAudio(clipToChangeTo);
            }
        }
    }

}
