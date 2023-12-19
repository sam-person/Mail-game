using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static REC_NPC.NPC_DialogueNode;
using System.Runtime.CompilerServices;

public class REC_Relay : Receiver
{
    

    public bool conditional = false;
    [ShowIf("conditional")]
    public List<NPC_DialogueCondition> conditions;
    [LabelText("@receiversLabel()")]
    public List<Receiver> receivers;
    [ShowIf("conditional"), LabelText("FALSE Recievers")]
    public List<Receiver> failedRecievers;

    [HorizontalGroup("Delay", LabelWidth = 100)]
    public bool useDelay = false;
    [ShowIf("useDelay"), HorizontalGroup("Delay", LabelWidth = 50), SuffixLabel("seconds")]
    public float delay = 1.0f;

    public override void Activate()
    {
        base.Activate();
        if (conditional)
        {
            if (CheckConditions())
            {
                ActivateTrueReceivers();
            }
            else {
                ActivateFalseReceivers();
            }
        }
        else {
            ActivateTrueReceivers();
        }
        
    }

    void ActivateTrueReceivers() {
        StartCoroutine(ActivateDelayed(useDelay ? delay : 0f, true));
    }

    void ActivateFalseReceivers()
    {
        StartCoroutine(ActivateDelayed(useDelay ? delay : 0f, false));
    }

    IEnumerator ActivateDelayed(float delay, bool trueRecievers) {
        if (useDelay) yield return new WaitForSeconds(delay);
        foreach (Receiver r in trueRecievers ? receivers : failedRecievers)
        {
            r.Activate();
        }
    }

    bool CheckConditions() {
        foreach (NPC_DialogueCondition condition in conditions)
        {
            if (!condition.Resolve())
            {
                Debug.Log("Condition Failed: " + condition.variable);
                return false;
            }
        }
        return true;
    }

    string receiversLabel() {
        return conditional ? "TRUE Recievers" : "Receivers";
    }
}
