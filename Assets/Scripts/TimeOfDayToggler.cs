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

    //Set to day time
    public void TimeSwitchMidday()
    {
        lightManager.LoadLightmapData(0);

        //Activate all day time objects
        foreach(GameObject gameObject in dayLight)
        {
            gameObject.SetActive(true);
        }

        //Deactivate all night time objects
        foreach (GameObject gameObject in nightLight)
        {
            gameObject.SetActive(false);
        }

        RenderSettings.ambientLight = ambientColorDay;
        RenderSettings.skybox = daySkyMaterial;
        isday = true;
        bakedLightIsOff = false;
    }

    //Set to night time
    public void TimeSwitchNight()
    {
        lightManager.LoadLightmapData(1);

        //Deactivate all day time objects
        foreach (GameObject gameObject in dayLight)
        {
            gameObject.SetActive(false);
        }

        //Activate all night time objects
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
