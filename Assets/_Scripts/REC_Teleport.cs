using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_Teleport : Receiver
{
    public bool inside = true;
    public Transform spawnPoint;
    public float xRot;
    public float yRot = 10f;


    public override void Activate()
    {
        base.Activate();
        PlayerInteractionHandler.instance.Teleport(this);
    }

}
