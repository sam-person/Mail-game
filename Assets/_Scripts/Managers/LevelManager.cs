using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public string levelName = "Level";

    public PlayerInteractionHandler player_Prefab;
    public Transform startPoint;
    public GameManager gameManger_Prefab;
    public InterfaceManager ui_Prefab;

    public Transform playerCameraTarget;

    private InterfaceManager ui;

    private void Awake()
    {
        if (instance != null) instance = this;

        //instantiate all required gameplay items
        //set up a bootsrap object
        GameObject bootStrap = new GameObject("[Bootstrap]");
        bootStrap.transform.SetAsFirstSibling();

        //spawn the GameManager
        GameManager gm = (GameManager)Instantiate(gameManger_Prefab);
        gm.transform.SetParent(bootStrap.transform);
        

        //spawn the UI
        if(ui == null)
        ui = (InterfaceManager)Instantiate(ui_Prefab);
        //ui.transform.SetParent(bootStrap.transform);

        //spawn the player
        PlayerInteractionHandler player = (PlayerInteractionHandler)Instantiate(player_Prefab, startPoint.position, startPoint.transform.rotation);
        player.transform.SetAsFirstSibling();

        //set the follow camera target
        gm.playerFollowCamera.Follow = player.thirdPersonController.CinemachineCameraTarget.transform;
        //gm.dialogueCamera.Follow = player.thirdPersonController.CinemachineCameraTarget.transform;
        //set the first target
        //gm.targetGroup.m_Targets[0].target = player.thirdPersonController.CinemachineCameraTarget.transform;
    }

    private void Update()
    {
        playerCameraTarget.position = PlayerInteractionHandler.instance.transform.position;
    }
}
