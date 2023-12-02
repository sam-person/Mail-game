using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraZoom : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    CinemachineComponentBase componentBase;

    float cameraDistance;
    [SerializeField] float sensitivity = 10f;
    [SerializeField] float maxCameraDistance = 20f;


    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Paused) return;
        if (componentBase == null)
        {
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }
        //if (GameManager.instance.currentSubarea != null && GameManager.instance.currentSubarea.forceZoomValue) {
        //    (componentBase as Cinemachine3rdPersonFollow).CameraDistance = GameManager.instance.currentSubarea.forcedZoomValue;
        //    return;
        //}

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
