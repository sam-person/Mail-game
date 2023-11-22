using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TRI_YarnCommand : Trigger
{
    [YarnCommand("Trigger")]
    public override void Activate()
    {
        base.Activate();
    }
}
