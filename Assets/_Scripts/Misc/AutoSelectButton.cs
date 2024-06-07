using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSelectButton : MonoBehaviour
{
    public Button button;
    public UIInputDevice _UIInputDevice;

    private void OnEnable()
    {
        if (_UIInputDevice != null)
        {
            //only preselect a button if using a controller
            if (!_UIInputDevice.usingKBM)
            {
                button.Select();
            }
        }
        else
        {
            Debug.LogError("_UIInputDevice was not set");
        }
    }
}
