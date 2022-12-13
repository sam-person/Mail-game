using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDoorInteract : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Transform MailRoomSpawn;


    public void DoorEnter()
    {
        player.transform.position = MailRoomSpawn.transform.position;
    }


    public void DoorExit()
    {

    }
}
