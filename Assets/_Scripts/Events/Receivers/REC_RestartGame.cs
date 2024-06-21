using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class REC_RestartGame : Receiver
{
    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.PauseButton_Quit();
    }
}
