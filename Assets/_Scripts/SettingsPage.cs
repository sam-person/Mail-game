using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPage : MonoBehaviour
{
    public List<SettingsOption> settings;




    public void Open()
    {
        foreach (SettingsOption setting in settings)
        {
            setting.Load();
        }
    }

    public void Close()
    {
        foreach (SettingsOption setting in settings)
        {
            setting.Save();
        }
    }

    private void OnEnable()
    {
        Open();
    }


    private void OnDisable()
    {
        Close();
    }

}
