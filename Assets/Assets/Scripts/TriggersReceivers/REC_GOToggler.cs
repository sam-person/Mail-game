using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class REC_GOToggler : Receiver
{

    public List<MeshRenderer> meshObjects;

    public enum ToggleType { Off, On, Toggle};

    public ToggleType toggleType = ToggleType.Off;

    public override void Activate()
    {
        base.Activate();

        switch (toggleType)
        {
            case ToggleType.Off:

                foreach (MeshRenderer go in meshObjects)
                {
                    go.enabled = false;
                    this.gameObject.GetComponent<Outline>().enabled = false;
                    this.gameObject.GetComponent<TRI_Focus>().enabled = false;
                }
                break;
            case ToggleType.On:

                foreach (MeshRenderer go in meshObjects)
                {
                    go.enabled = true;
                    this.gameObject.GetComponent<Outline>().enabled = true;
                    this.gameObject.GetComponent<TRI_Focus>().enabled = true;
                }
                break;
            case ToggleType.Toggle:

                foreach (MeshRenderer go in meshObjects)
                {
                    go.enabled = (!enabled);
                }
                break;
        }


    }
}
