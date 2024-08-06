using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    public GameObject blackOutSquare;
    Image blackoutImage;

    //Coroutine fadePauseCoroutine;
    //Coroutine fadeBlackOutSquare;
    bool coroutineIsRunning = false;
    Color objectColor;
    float fadeAmount;


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Debug.Log("fade color = " + blackoutImage.color.a + ". coroutineIsRunning = " + coroutineIsRunning);
        //}
    }

    private void Start()
    {
        blackoutImage = blackOutSquare.GetComponent<Image>();
        Color objectColor = blackoutImage.color;
        blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.g, blackoutImage.color.b, 1);
        Fade(false, 1f);
    }

    public void Fade(bool fadeToBlack = true, float fadeSpeed = 5) 
    {
        if (!coroutineIsRunning)
        {
            //fadeBlackOutSquare =
            StartCoroutine(FadeBlackOutSquare(fadeToBlack, fadeSpeed));
        }
        else
        {
            //StopCoroutine(fadeBlackOutSquare);
            //coroutineIsRunning = false;
            blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.g, blackoutImage.color.b, 0);
        }
    }

    void FadeToBlack(float fadeSpeed)
    {
        fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        blackoutImage.color = objectColor;
    }

    void FadeToClear(float fadeSpeed)
    {
        fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        blackoutImage.color = objectColor;
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = 5)
    {
        coroutineIsRunning = true;
        blackoutImage.color = new Color(objectColor.r, objectColor.g, objectColor.b, 0);

        if (fadeToBlack)
        {
            //0 is clear
            //1 is black

            Debug.Log("starting fade to black - fade color = " + blackoutImage.color.a);

            // Fade to Black
            while (blackoutImage.color.a < 1)
            {
                FadeToBlack(fadeSpeed);
                yield return null;
            }

            Debug.Log("faded to black - fade color = " + blackoutImage.color.a);
            GameManager.instance.OnMidFade();
            //stay black for this .5 seconds
            yield return new WaitForSeconds(0.5f);

            // Fade to Clear
            while (blackoutImage.color.a > 0)
            {
                FadeToClear(fadeSpeed);
                yield return null;
            }

            // Ensure the alpha is exactly 0, fully clear
            blackoutImage.color = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
            Debug.Log("faded to clear - fade color = " + blackoutImage.color.a);
            coroutineIsRunning = false;
        }
        else
        {
            Debug.Log("else not fading to black");
            while (blackoutImage.color.a > 0)
            {
                FadeToClear(fadeSpeed);
                yield return null;
            }

            coroutineIsRunning = false;
            yield return new WaitForEndOfFrame();
        }
    }
}
