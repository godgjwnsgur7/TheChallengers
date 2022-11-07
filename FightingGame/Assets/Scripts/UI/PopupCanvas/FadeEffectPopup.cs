using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeEffectPopup : PopupUI
{
    [SerializeField] Image backgroundImage;

    Action fadeInCallBack = null;
    Action fadeOutCallBack = null;

    public void Open_FadeInEffect(Action _fadeInCallBack, float _fadeInTime)
    {
        fadeInCallBack = _fadeInCallBack;

        backgroundImage.color = new Color(0, 0, 0, 1);

        this.gameObject.SetActive(true);

        StartCoroutine(IFadeInEffect(_fadeInTime));
    }

    public void Open_FadeOutEffect(Action _fadeOutCallBack, float _fadeOutTime)
    {
        fadeOutCallBack = _fadeOutCallBack;

        backgroundImage.color = new Color(0, 0, 0, 0);

        this.gameObject.SetActive(true);

        StartCoroutine(IFadeOutEffect(_fadeOutTime));
    }

    IEnumerator IFadeInEffect(float _fadeInTime)
    {
        if(fadeInCallBack != null)
            fadeInCallBack();

        Color tempColor = backgroundImage.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeInTime;
            backgroundImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        backgroundImage.color = tempColor;
    }

    IEnumerator IFadeOutEffect(float _fadeOutTime)
    {
        Color tempColor = backgroundImage.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeOutTime;
            backgroundImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        backgroundImage.color = tempColor;
        if (fadeInCallBack != null)
            fadeOutCallBack();
    }
}
