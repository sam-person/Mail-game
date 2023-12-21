using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInBuild : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!(Application.isEditor || Debug.isDebugBuild)) {//if we're notin the editor or in a debug build
            gameObject.SetActive(false);
        }
    }

}
