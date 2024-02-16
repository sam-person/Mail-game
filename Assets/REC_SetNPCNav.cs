using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class REC_SetNPCNav : Receiver
{
    public REC_NPC npc;


    [BoxGroup("AutoNav")] public bool SetAutoNavState = false;
    [BoxGroup("AutoNav"), ShowIf("SetAutoNavState")] public bool navState = true;

    [BoxGroup("GoToNode")] public bool GoToNode = false;
    [BoxGroup("GoToNode"), ShowIf("GoToNode")] public int node = 0;

    public override void Activate()
    {
        base.Activate();

        if (SetAutoNavState) {
            npc.SetAutoNavigation(navState);
        }

        if (GoToNode) {
            npc.GoToNode(node);
        }
    }
}
