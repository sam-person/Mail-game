using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnInputDeviceChanged_Image : MonoBehaviour
{
    public Sprite[] KBM_Sprites;
    public Sprite[] PS_Sprites;
    public Sprite[] XB_Sprites;

    public Image[] images;
    public void OnInputDeviceChanged(string device)
    {
        //if(device == "PS")
        //{
            //for (int i = 0; i < images.Length; i++)
            //{
            //    images[i].sprite = PS_Sprites[i];
            //}
        Debug.Log("changing image sprite to PS");

        images[0].sprite = PS_Sprites[0];
        //}
        //else if (device == "XB")
        //{
        //    for (int i = 0; i < images.Length; i++)
        //    {
        //        images[i].sprite = XB_Sprites[i];
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < images.Length; i++)
        //    {
        //        images[i].sprite = KBM_Sprites[i];
        //    }
        //}
    }
}
