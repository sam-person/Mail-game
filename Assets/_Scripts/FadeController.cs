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
        Image blackoutImage = blackOutSquare.GetComponent<Image>();
        blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.g, blackoutImage.color.b, 1);
        Fade(false, 1f);
    }

    public void Fade(bool fadeToBlack = true, float fadeSpeed = 5) {
        StartCoroutine(FadeBlackOutSquare(fadeToBlack, fadeSpeed));
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = 5)
    {

        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;


        if (fadeToBlack)
        {
            //bool fadefinished = false;

            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            if(blackOutSquare.GetComponent<Image>().color.a >= 1)
            {
                GameManager.instance.OnMidFade();
                yield return new WaitForSeconds(0.5f);
                while (blackOutSquare.GetComponent<Image>().color.a > 0)
                {
                    fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
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
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }


    }


   

    
}
