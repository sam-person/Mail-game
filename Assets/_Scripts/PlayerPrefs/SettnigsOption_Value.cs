using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettnigsOption_Value : SettingsOption
{
    public Slider mySlider;

    public override void Load()
    {
        base.Load();
        mySlider.value = PlayerPrefs.GetFloat(PlayerPrefString, 1);

    }

    public override void Save()
    {
        base.Save();
        PlayerPrefs.SetFloat(PlayerPrefString, mySlider.value);
    }
}
