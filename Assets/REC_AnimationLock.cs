using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_AnimationLock : Receiver
{
    public Transform lockPivot;
    public string animTrigger = "Sit";

    public override void Activate()
    {
        base.Activate();
        PlayerInteractionHandler.instance.thirdPersonController.transform.position = lockPivot.position;
        PlayerInteractionHandler.instance.thirdPersonController.transform.rotation = lockPivot.rotation;
        PlayerInteractionHandler.instance.EnterAnimationLock(animTrigger);
    }
}
