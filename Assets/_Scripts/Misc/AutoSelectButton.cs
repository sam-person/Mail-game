using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSelectButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button.Select();
    }
}
