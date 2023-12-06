using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterSeconds : MonoBehaviour
{
    public float secondsUntilDelete = 1.2f; 

    void Awake()
    {
        Invoke("DeleteGameObject", secondsUntilDelete);
    }

    void DeleteGameObject()
    {
        Destroy(gameObject);
    }
}
