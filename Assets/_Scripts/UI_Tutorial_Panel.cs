using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class UI_Tutorial_Panel : MonoBehaviour
{
    public float aliveTime;
    float _timer;
    float _scaleTimer;

    public AnimationCurve fadeOutCurve;
    public AnimationCurve ScaleInCurve;
    public float ScaleInSpeed = 2f;

    public Image image;
    public TextMeshProUGUI tutorialText;
    public CanvasGroup canvasGroup;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.transform.localScale = new Vector3(0, 0);
        _timer = aliveTime;
        _scaleTimer = ScaleInSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0f) {
            _timer -= Time.deltaTime;
            SetFadeAmount();
            if (_timer <= 0f) {
                Destroy(this.gameObject);
                
            }
        }

        if(_scaleTimer > 0f)
        {
            _scaleTimer -= Time.deltaTime;
            ScaleIn();
        }
    }

    void SetFadeAmount() {
        float timeOutPercent = Mathf.InverseLerp(aliveTime, 0, _timer);
        canvasGroup.alpha = fadeOutCurve.Evaluate(timeOutPercent);
    }

    void ScaleIn()
    {
        float timeOutPercent = Mathf.InverseLerp(ScaleInSpeed, 0, _scaleTimer);
        canvasGroup.transform.localScale = new Vector3(ScaleInCurve.Evaluate(timeOutPercent), ScaleInCurve.Evaluate(timeOutPercent));
    }

    public void SetPanel(Sprite _image, string _text, float _time) {
        image.sprite = _image;
        tutorialText.text = _text;
        aliveTime = _time;
        _timer = aliveTime;
    }
}
