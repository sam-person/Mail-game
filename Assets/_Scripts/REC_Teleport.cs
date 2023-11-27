using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class REC_Teleport : Receiver
{
    public bool inside = true;
    public Transform spawnPoint;
    public float xRot;
    public float yRot = 10f;

    public enum TeleportType {Enter, Exit, None };
    public TeleportType teleType = TeleportType.Exit;
    [ShowIf("teleType", TeleportType.Enter)]
    public Subarea subarea;

    public override void Activate()
    {
        base.Activate();
        PlayerInteractionHandler.instance.Teleport(this);
        switch (teleType)
        {
            case TeleportType.Enter:
                GameManager.instance.currentSubarea = subarea;
                break;
            case TeleportType.Exit:
                GameManager.instance.currentSubarea = null;
                break;
            case TeleportType.None:
                break;
        }
    }

}
