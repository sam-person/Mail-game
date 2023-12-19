using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToggleObjectsWithKeycode : MonoBehaviour
{
    public List<GameObject> objectsToToggle;
    public KeyCode toggleKey = KeyCode.F;

#if UNITY_EDITOR
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
#endif
}
