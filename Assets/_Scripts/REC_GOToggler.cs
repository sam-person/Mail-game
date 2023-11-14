using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_GOToggler : Receiver
{

    public List<GameObject> gameObjects;

    public enum ToggleType { Off, On, Toggle };

    public ToggleType toggleType = ToggleType.Off;

    public override void Activate()
    {
        base.Activate();

        switch (toggleType)
        {
            case ToggleType.Off:

                foreach (GameObject go in gameObjects)
                {
                    go.SetActive(false);
                }
                break;
            case ToggleType.On:

                foreach (GameObject go in gameObjects)
                {
                    go.SetActive(true);
                }
                break;
            case ToggleType.Toggle:

                foreach (GameObject go in gameObjects)
                {
                    go.SetActive(!go.activeSelf);
                }
                break;
        }


    }
}