using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class REC_SetCameraTarget : Receiver
{
    public Transform target;
    public override void Activate()
    {
        base.Activate();
        //GameManager.instance.SetDialogueCameraTarget(1, target);
    }
}
