using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private CharacterController characterController;
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        characterController.Move(move * Time.deltaTime * MoveSpeed);
    }
}
