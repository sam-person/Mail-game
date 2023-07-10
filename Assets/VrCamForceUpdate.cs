using UnityEngine;
using Cinemachine;
using System;

public class VrCamForceUpdate : MonoBehaviour
{
    public static Action VrCamForceResetAction;
    CinemachineVirtualCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        Debug.Log("The item in cam is " + cam);
    }
    private void OnEnable()
    {
        VrCamForceResetAction += ForceResetCam;
    }
    private void OnDisable()
    {
        VrCamForceResetAction -= ForceResetCam;
    }
    public void ForceResetCam()
    {
        cam.PreviousStateIsValid = false;
    }
}
