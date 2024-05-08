using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightingSettings", menuName = "Custom/Lighting Settings")]
public class LightingSettings : ScriptableObject
{
    public Color realtimeShadowColor = Color.blue;
    [ColorUsage(true, true)]
    public Color ambientLightColor = Color.white;
    public Cubemap reflectionsCubemap;
}
