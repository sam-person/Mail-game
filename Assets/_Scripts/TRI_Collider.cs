using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRI_Collider : Trigger
{

    private void OnTriggerEnter(Collider other)
    {
        base.Activate();
    }

}
