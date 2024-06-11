using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class REC_TutorialPanel : Receiver
{
//    [PreviewField(Alignment = ObjectFieldAlignment.Left)]
    public float tutorialTime;

    public Sprite tutorialImage;
    public string tutorialText;
    public Sprite tutorialImagePS;
    public string tutorialTextPS;
    public Sprite tutorialImageXB;
    public string tutorialTextXB;

    public override void Activate()
    {
        base.Activate();
        InterfaceManager.instance.SpawnTutorialPanel(tutorialImage, tutorialImagePS, tutorialText, tutorialTextPS, tutorialTime);
    }
}
