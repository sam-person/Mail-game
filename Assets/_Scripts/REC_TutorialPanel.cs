using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class REC_TutorialPanel : Receiver
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public Sprite tutorialImage;
    public string tutorialText;
    public float tutorialTime;

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.SpawnTutorialPanel(tutorialImage, tutorialText, tutorialTime);
    }
}
