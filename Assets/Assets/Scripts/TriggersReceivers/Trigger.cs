using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Trigger : MonoBehaviour
{
    public List<Receiver> Receivers;
    public bool oneShot = false;
    private bool alreadyTriggered = false;

    [Button]
    public void Activate()
    {
        if(oneShot && alreadyTriggered) 
        {
            Debug.Log("Already triggered before");
        }
        else
        {
            foreach (Receiver r in Receivers)
            {
                r.Activate();
            }
            alreadyTriggered = true;

        }
    }
}
