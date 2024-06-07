using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnInputDeviceChanged_Image : MonoBehaviour
{
    public Sprite KBM_Sprite;
    public Sprite PS_Sprite;
    public Sprite XB_Sprite;

    public Image image;
    public void OnInputDeviceChanged(string device)
    {
        if (device == "PS")
        {
            image.sprite = PS_Sprite;
        }
        else if (device == "XB")
        {
            image.sprite = XB_Sprite;
        }
        else
        {
            image.sprite = KBM_Sprite;
        }
    }
}
