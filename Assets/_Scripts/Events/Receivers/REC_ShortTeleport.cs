using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_ShortTeleport : Receiver
{

    public Transform positionToTeleport;

	public override void Activate()
    {
        base.Activate();
        PlayerInteractionHandler.instance.thirdPersonController.transform.position = positionToTeleport.position;
        PlayerInteractionHandler.instance.thirdPersonController.transform.rotation = positionToTeleport.rotation;
    }
}
