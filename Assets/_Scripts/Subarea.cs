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

    private void Awake()
    {
        subareaCamera.gameObject.SetActive(false);
    }

    public void OnEnterSubarea() {
        subareaCamera.gameObject.SetActive(true);
    }

    public void OnExitSubarea()
    {
        subareaCamera.gameObject.SetActive(false);
    }
}
