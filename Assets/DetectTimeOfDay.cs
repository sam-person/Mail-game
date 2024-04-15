using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectTimeOfDay : MonoBehaviour
{
    public GameObject dayTimeBGM;
    public GameObject nightTimeBGM;


    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Contains("Evening"))
        {
            nightTimeBGM.SetActive(true);
            dayTimeBGM.SetActive(false);
        }
        else
        {
            nightTimeBGM.SetActive(false);
            dayTimeBGM.SetActive(true);
        }
    }
}
