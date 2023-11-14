using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RotationTest : MonoBehaviour
{

    private bool playOnce = false;
    public CinemachineVirtualCamera vcam;
    public CinemachineVirtualCamera target;
    Coroutine camRotCoroutine;
    public GameObject currentRotation;
    public GameObject mainRotation;

    private IEnumerator CameraRotator()
    {

        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!playOnce)
            {

                
                playOnce = true;


            }

            playOnce = false;

        }
    }
}
