using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class REC_SceneChange : Receiver
{
    public string scene;
    public bool instant = false;

    public override void Activate()
    {
        base.Activate();
        if (instant)
        {
            ChangeScene();
        }
        else
        {
            GameManager.instance.StartSceneChange(this);
        }
    }

    public void ChangeScene() {
        SceneManager.LoadScene(scene);
    }
}
