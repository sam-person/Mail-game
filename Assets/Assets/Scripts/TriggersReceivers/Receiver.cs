using Sirenix.OdinInspector.Editor.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    public virtual void Activate()
    {
        Debug.Log(name + " Activated");
    }
}
