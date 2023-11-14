using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    public GameObject blackOutSquare;

    Coroutine fadePauseCoroutine;


    // Update is called once per frame
    void Update()
    {

    }

    private void Start()
    {
        GameManager.gamePaused += PauseFadeController;
    }

    public void Fade(bool fadeToBlack = true, int fadeSpeed = 5) {
        StartCoroutine(FadeBlackOutSquare(fadeToBlack, fadeSpeed));
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 5)
    {

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;


        if (fadeToBlack)
        {
            //bool fadefinished = false;

            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.unscaledDeltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            if(blackOutSquare.GetComponent<Image>().color.a >= 1)
            {
                yield return new WaitForSeconds(0.5f);
                while (blackOutSquare.GetComponent<Image>().color.a > 0)
                {
                    fadeAmount = objectColor.a - (fadeSpeed * Time.unscaledDeltaTime);
                    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                    blackOutSquare.GetComponent<Image>().color = objectColor;
                    yield return null;
                }
            }
        }
        else
        {
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.unscaledDeltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }


    }


    public void PauseFadeController(bool isPaused)
    {
        if (isPaused)
        {
            fadePauseCoroutine = StartCoroutine(FadePauseSquare(true));
        }
        else
        {
            fadePauseCoroutine = StartCoroutine(FadePauseSquare(false));
        }
    }

    public IEnumerator FadePauseSquare(bool fadeToBlack = true, int fadeSpeed = 5)
    {

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;


        if (fadeToBlack)
        {
            //bool fadefinished = false;

            while (blackOutSquare.GetComponent<Image>().color.a < 0.65)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.unscaledDeltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            //if (blackOutSquare.GetComponent<Image>().color.a >= 1)
            //{
            //    yield return new WaitForSeconds(0.5f);
            //    while (blackOutSquare.GetComponent<Image>().color.a > 0)
            //    {
            //        fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            //        Debug.Log("I am decreasing the black");
            //        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            //        blackOutSquare.GetComponent<Image>().color = objectColor;
            //        yield return null;
            //    }
            //}
        }
        else
        {
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.unscaledDeltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }


    }
}
