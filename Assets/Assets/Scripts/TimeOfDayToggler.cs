using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TimeOfDayToggler : MonoBehaviour
{
    [SerializeField]
    bool toggleBakedLighting;
    bool bakedLightIsOff;

    [SerializeField]
    bool toggleTime;

    public BakeryLightmapManager lightManager;

    public GameObject[] dayLight;
    public GameObject[] nightLight;
    public Material daySkyMaterial;
    public Material nightSkyMaterial;

    private bool isday = true;

    [ColorUsage(true, true)]
    public Color ambientColorDay;
    public Color ambientColorNight;

    void Update()
    {
#if UNITY_EDITOR
        if (toggleTime)
        {
            if (isday)
            {
                TimeSwitchNight();
            }
            else
            {
                TimeSwitchMidday();
            }
            toggleTime = false;
        }

        if(toggleBakedLighting)
        {
            if(bakedLightIsOff)
            {
                TimeSwitchMidday();
            }
            else
            {
                lightManager.LoadLightmapData(2);
                bakedLightIsOff = true;
            }

            toggleBakedLighting = false;
        }
       
#endif
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(isday)
            {
                TimeSwitchNight();
            }
            else
            {
                TimeSwitchMidday();
            }
        }
    }

    public void TimeSwitchMidday()
    {
        lightManager.LoadLightmapData(0);

        foreach(GameObject gameObject in dayLight)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in nightLight)
        {
            gameObject.SetActive(false);
        }

        RenderSettings.ambientLight = ambientColorDay;
        RenderSettings.skybox = daySkyMaterial;
        isday = true;
        bakedLightIsOff = false;
    }

    public void TimeSwitchNight()
    {
        lightManager.LoadLightmapData(1);

        foreach (GameObject gameObject in dayLight)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in nightLight)
        {
            gameObject.SetActive(true);
        }

        RenderSettings.ambientLight = ambientColorNight;
        RenderSettings.skybox = nightSkyMaterial;
        isday = false;
        bakedLightIsOff = false;
    }
}
