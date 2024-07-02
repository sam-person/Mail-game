using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorExteriorLightingSwitcher : MonoBehaviour
{
    public LightingSettings interiorLightingSettings;
    public LightingSettings exteriorLightingSettings;

    public GameObject interiorSun;
    public GameObject exteriorSun;

    public bool startInside = false;

    private void Awake()
    {
        if(startInside)
        {
            SwitchToInteriorLighting();
        }
        else
        {
            SwitchToExteriorLighting();
        }
    }

    public void SwitchToInteriorLighting()
    {
        if (interiorLightingSettings != null)
        {
            RenderSettings.subtractiveShadowColor = interiorLightingSettings.realtimeShadowColor;
            RenderSettings.ambientLight = interiorLightingSettings.ambientLightColor;
            RenderSettings.customReflection = interiorLightingSettings.reflectionsCubemap;
        }
        else
        {
            Debug.LogError("interiorLightingSettings is not assigned");
        }

        if(exteriorSun != null)
        {
            exteriorSun.SetActive(false);
        }

        if(interiorSun != null)
        {
            interiorSun.SetActive(true);
        }
    }

    public void SwitchToExteriorLighting()
    {
        if(exteriorLightingSettings != null)
        {
            RenderSettings.subtractiveShadowColor = exteriorLightingSettings.realtimeShadowColor;
            RenderSettings.ambientLight = exteriorLightingSettings.ambientLightColor;
            RenderSettings.customReflection = exteriorLightingSettings.reflectionsCubemap;
        }
        else
        {
            Debug.LogError("exteriorLightingSettings is not assigned");
        }

        if (exteriorSun != null)
        {
            exteriorSun.SetActive(true);
        }

        if (interiorSun != null)
        {
            interiorSun.SetActive(false);
        }
    }
}
