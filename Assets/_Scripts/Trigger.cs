using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static REC_NPC.NPC_DialogueNode;

public class Trigger : MonoBehaviour
{
    public List<Receiver> Receivers;
    public bool oneShot = false;
    private bool alreadyTriggered = false;

    public List<NPC_DialogueCondition> conditions;

    [Button]
    public virtual void Activate()
    {
        if(oneShot && alreadyTriggered) 
        {
            Debug.Log("Already triggered before");
            return;
        }
        foreach (NPC_DialogueCondition condition in conditions) {
            if (!condition.Resolve()) {
                Debug.Log("Condition Failed: " + condition.variable);
                return;
            }
        }

            foreach (Receiver r in Receivers)
            {
                r.Activate();
            }
            alreadyTriggered = true;

        
    }
}
