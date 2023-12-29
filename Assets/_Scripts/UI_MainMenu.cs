using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[LabelText("For now, you press any button to load the main scene")]
public class UI_MainMenu : MonoBehaviour
{
    public string sceneToLoad;
    public float fadeoutTime = 2.0f;
    public Image fader;
    float _timer;
    bool fading = false;

    private void Start()
    {
        _timer = fadeoutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !fading) {
            fading = true;
        }

        if (fading && _timer > 0) {
            MainMenu_Play();
        }
    }

    public void MainMenu_Play()
    {
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
