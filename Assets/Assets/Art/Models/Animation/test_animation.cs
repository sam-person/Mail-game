using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_animation : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            animator.SetBool("isWalking", true);
        }
    }
}
