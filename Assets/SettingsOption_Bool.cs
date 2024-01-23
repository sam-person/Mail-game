using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOption_Bool : SettingsOption
{
    public Toggle myToggle;

    public override void Load()
    {
        base.Load();
        myToggle.isOn = PlayerPrefs.GetInt(PlayerPrefString) == 1;

    }

    public override void Save()
    {
        base.Save();
        PlayerPrefs.SetInt(PlayerPrefString, myToggle.isOn ? 1 : 0);
    }
}
