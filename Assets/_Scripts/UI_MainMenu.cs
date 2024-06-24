using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[LabelText("For now, you press any button to load the main scene")]
public class UI_MainMenu : MonoBehaviour
{
    public string sceneToLoad;
    public float fadeoutTime = 2.0f;
    public Image fader;
    float _timer;
    bool fading = false;
    public UIInputDevice uIInputDevice;
    public bool hasNotSwitchedBackToGamepad;
    public AutoSelectButton autoSelectButton;

    private void Start()
    {
        _timer = fadeoutTime;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.anyKeyDown && !fading) {
        //    fading = true;
        //}

        if (fading && _timer > 0) {
            MainMenu_Play();
        }


        if(!uIInputDevice.usingKBM)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if(hasNotSwitchedBackToGamepad)
            {
                autoSelectButton.SelectMainMenuPlayButton();
                hasNotSwitchedBackToGamepad = false;
            }
        }
        else 
        {
            hasNotSwitchedBackToGamepad = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void MainMenu_Play()
    {
        fading = true;
        _timer -= Time.deltaTime;
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, Mathf.InverseLerp(fadeoutTime, 0f, _timer));
        if (_timer <= 0f)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void MainMenu_Quit()
    {
        Application.Quit();
        Debug.Log("Quiting game");
    }
}
