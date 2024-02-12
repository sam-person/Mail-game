using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOption_List : SettingsOption
{
    public int index;
    public List<string> displaysValues;
    public TMPro.TextMeshProUGUI myText;

    public override void Load()
    {
        base.Load();
        index = PlayerPrefs.GetInt(PlayerPrefString);
        UpdateDisplay();


    }

    public override void Save()
    {
        base.Save();
        PlayerPrefs.SetInt(PlayerPrefString, index);
    }


    public void Forward()
    {
        index++;
        if(index > displaysValues.Count - 1)
        {
            index = 0;
        }
        UpdateDisplay();
    }

    public void Back()
    {
        index--;
        if (index < 0)
        {
            index = displaysValues.Count-1;
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        myText.text = displaysValues[index].ToString();

        
    }


}
