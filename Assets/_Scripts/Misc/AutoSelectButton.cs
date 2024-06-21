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

    private void OnEnable()
    {
        if(addDelay)
        {
            StartCoroutine(DelayButtonSelection());
        }
        else
        {
            SelectButton();
        }
    }

    private IEnumerator DelayButtonSelection()
    {
        yield return new WaitForSeconds(delayAmount);
        SelectButton();
    }

    private void SelectButton()
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
