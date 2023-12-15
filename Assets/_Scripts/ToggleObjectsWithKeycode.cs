using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectsWithKeycode : MonoBehaviour
{
    public List<GameObject> objectsToToggle;
    public KeyCode toggleKey = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            // Toggle the visibility of each GameObject in the list
            foreach (GameObject obj in objectsToToggle)
            {
                if (obj != null)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
    }
}
