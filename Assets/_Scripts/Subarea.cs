using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Subarea : MonoBehaviour
{
    //public bool forceZoomValue = true;
    //public float forcedZoomValue = 4f;

    public string subareaName = "Inside";
    public CinemachineVirtualCamera subareaCamera;
    public bool useCamera = false;

    private void Awake()
    {
        if(subareaCamera)subareaCamera.gameObject.SetActive(false);
    }

    public void OnEnterSubarea() {
        if (useCamera && subareaCamera)
        {
            subareaCamera.gameObject.SetActive(true);
        }
    }

    public void OnExitSubarea()
    {
        if (subareaCamera && useCamera) subareaCamera.gameObject.SetActive(false);
    }
}
