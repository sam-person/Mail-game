using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REC_Teleport : Receiver
{
    //private variables
    private GameObject player;
    private Camera mainCamera;
    private GameObject fadeObj;
    private FadeController fadeController;
    public ThirdPersonController tpc;

    //public variables
    public Transform doorSpawn;
    public bool goingInside = false;
    public float cameraYaw = 0;
    public float cameraPitch = 0;

    //coroutines
    Coroutine teleportCoroutine;
    Coroutine fadeInCoroutine;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        fadeObj = GameObject.FindWithTag("FadeController");
        fadeController = fadeObj.GetComponent<FadeController>();
        mainCamera = Camera.main;
    }

    private IEnumerator TeleportPlayer()
    {
        if (goingInside)
        {
            yield return new WaitForSeconds(0.2f);
            player.transform.rotation = doorSpawn.transform.rotation;
            player.transform.position = doorSpawn.transform.position;
            VrCamForceUpdate.VrCamForceResetAction?.Invoke();
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            tpc.SetCameraAngle(cameraYaw, cameraPitch);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            base.Activate();
            player.transform.rotation = doorSpawn.transform.rotation;
            player.transform.position = doorSpawn.transform.position;
            VrCamForceUpdate.VrCamForceResetAction?.Invoke();
            mainCamera.clearFlags = CameraClearFlags.Skybox;
            tpc.SetCameraAngle(cameraYaw, cameraPitch);
        }
    }

    public override void Activate()
    {
        base.Activate();
        teleportCoroutine = StartCoroutine(TeleportPlayer());
        fadeInCoroutine = StartCoroutine(fadeController.FadeBlackOutSquare());
        
    }
      

}
