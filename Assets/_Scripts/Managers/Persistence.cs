using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    #region Init

    public static Persistence Instance;

    //Make sure to spawn persistence at startup if it doesn't already exist
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInit()
    {
        if (FindObjectOfType<Persistence>() != null)
            return;

        //var go = new GameObject { name = "[ " + typeof(Persistence).ToString() + " ]" };
        //go.AddComponent<Persistence>();
        GameObject go = GameObject.Instantiate(Resources.Load("[Persistence]") as GameObject);

        DontDestroyOnLoad(go);

        if (Instance == null)
        {
            Instance = go.GetComponent<Persistence>();
        }
        //Debug.Log("AutoSpawning " + typeof(Persistence).ToString());
    }

    #endregion

    
}
