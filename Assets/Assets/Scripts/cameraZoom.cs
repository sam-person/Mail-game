using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    CinemachineComponentBase componentBase;

    float cameraDistance;
    [SerializeField] float sensitivity = 10f;
    [SerializeField] float maxCameraDistance = 20f;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.gamePaused += czIsDisabled;
    }

    public void czIsDisabled(bool isDisabled)
    {
        if (isDisabled)
        {
            this.enabled = false;
        }
        else
        {
            this.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (componentBase == null)
        {
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            if (componentBase is Cinemachine3rdPersonFollow)
            {

                if (cameraDistance < 0)
                {
                    (componentBase as Cinemachine3rdPersonFollow).CameraDistance -= cameraDistance;
                    if ((componentBase as Cinemachine3rdPersonFollow).CameraDistance > maxCameraDistance)
                    {
                        (componentBase as Cinemachine3rdPersonFollow).CameraDistance = maxCameraDistance;
                    }

                }

                else
                {
                    (componentBase as Cinemachine3rdPersonFollow).CameraDistance -= cameraDistance;
                    if ((componentBase as Cinemachine3rdPersonFollow).CameraDistance < 4)
                    {
                        (componentBase as Cinemachine3rdPersonFollow).CameraDistance = 4;
                    }
                }
            }
        }
    }


}
