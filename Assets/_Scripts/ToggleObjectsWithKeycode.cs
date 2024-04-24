using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StarterAssets;

public class ToggleObjectsWithKeycode : MonoBehaviour
{
    public List<GameObject> objectsToToggle;
    public KeyCode toggleKey = KeyCode.F;
    private bool isFast = false;
    private ThirdPersonController tpc;
    private GameObject playerObj;

    public void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        tpc = playerObj.GetComponent<ThirdPersonController>();
    }

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
            if (!isFast)
            {
                tpc.MoveSpeed = 20;
                isFast = true;
            }
            else
            {
                tpc.MoveSpeed = 2;
                isFast = false;
            }

        }
    }

}
