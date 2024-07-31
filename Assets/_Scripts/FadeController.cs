using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    public GameObject blackOutSquare;
    Image blackoutImage;

    Coroutine fadePauseCoroutine;


    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    Debug.Log("fade color = " + blackoutImage.color.a);
        //}
    }

    private void Start()
    {
        blackoutImage = blackOutSquare.GetComponent<Image>();
        blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.g, blackoutImage.color.b, 1);
        Fade(false, 1f);
    }

    public void Fade(bool fadeToBlack = true, float fadeSpeed = 5) {
        StartCoroutine(FadeBlackOutSquare(fadeToBlack, fadeSpeed));
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = 5)
    {

        Color objectColor = blackoutImage.color;
        float fadeAmount;


        if (fadeToBlack)
        {
            //0 is clear
            //1 is black

            Debug.Log("starting fade to black - fade color = " + blackoutImage.color.a);

            // First while loop to fade in
            while (blackoutImage.color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackoutImage.color = objectColor;
                yield return null;
            }

            Debug.Log("faded to black - fade color = " + blackoutImage.color.a);

            // Ensure the alpha is exactly 1
            blackoutImage.color = new Color(objectColor.r, objectColor.g, objectColor.b, 1);
            GameManager.instance.OnMidFade();
            yield return new WaitForSeconds(0.5f);
            Debug.Log("waited at black - fade color = " + blackoutImage.color.a);

            // Second while loop to fade out
            while (blackoutImage.color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackoutImage.color = objectColor;
                yield return null;
            }

            // Ensure the alpha is exactly 0
            Debug.Log("faded out of black - fade color = " + blackoutImage.color.a);
            blackoutImage.color = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
            Debug.Log("set value to 0 - fade color = " + blackoutImage.color.a);
        }
        else
        {
            Debug.Log("else not fading to black");
            while (blackoutImage.color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackoutImage.color = objectColor;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }

    }
}
