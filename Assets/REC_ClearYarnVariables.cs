using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class REC_ClearYarnVariables : Receiver
{

    public override void Activate()
    {
        base.Activate();
        Yarn.Unity.InMemoryVariableStorage yarnStorage = FindObjectOfType<Yarn.Unity.InMemoryVariableStorage>();
        yarnStorage.Clear();
    }
}
