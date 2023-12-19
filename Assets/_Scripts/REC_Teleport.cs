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
    }

}
