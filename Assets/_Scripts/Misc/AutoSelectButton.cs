using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSelectButton : MonoBehaviour
{
    public Button button;
    public UIInputDevice _UIInputDevice;

    public bool addDelay = false;
    public float delayAmount = 0.1f;
    public bool alwaysSelectButton = false;

    private void OnEnable()
    {
        if(addDelay)
        {
            StartCoroutine(DelayButtonSelection());
        }
        else
        {
            SelectMainMenuPlayButton();
        }
    }

    private IEnumerator DelayButtonSelection()
    {
        yield return new WaitForSeconds(delayAmount);
        SelectMainMenuPlayButton();
    }

    public void SelectMainMenuPlayButton()
    {
        if (_UIInputDevice != null)
        {
            if (alwaysSelectButton)
            {
                button.Select();
            }
            //only preselect a button if using a controller
            else if (!_UIInputDevice.usingKBM)
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
