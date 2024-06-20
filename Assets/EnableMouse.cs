using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Show the mouse cursor
        if (GameManager.instance.isUsingKBM)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
