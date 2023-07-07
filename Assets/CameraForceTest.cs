using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraForceTest : MonoBehaviour
{

    public CinemachineVirtualCamera vcam;
    public GameObject transHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            vcam.ForceCameraPosition(transHolder.transform.position, transHolder.transform.rotation);
            vcam.PreviousStateIsValid = false;

            
        }
    }
}
