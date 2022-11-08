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

    Coroutine fadeInCoroutine = null;
    Coroutine fadeOutCoroutine = null;

    private void Awake()
    {
        
    }

    /// <summary>
    /// 서서히 검은 화면이 나타남
    /// </summary>
    public void Play_FadeInEffect(Action _fadeInCallBack, float _fadeInTime)
    {
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);

        fadeInCallBack = _fadeInCallBack;

        backgroundImage.color = new Color(0, 0, 0, 0);

        this.gameObject.SetActive(true);

        fadeInCoroutine = StartCoroutine(IFadeInEffect(_fadeInTime));
    }

    /// <summary>
    /// 서서히 검은 화면이 사라짐
    /// </summary>
    public void Play_FadeOutEffect(Action _fadeOutCallBack, float _fadeOutTime)
    {
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);

        fadeOutCallBack = _fadeOutCallBack;

        backgroundImage.color = new Color(0, 0, 0, 1);

        fadeOutCoroutine = StartCoroutine(IFadeOutEffect(_fadeOutTime));
    }

    IEnumerator IFadeInEffect(float _fadeInTime)
    {
        Color tempColor = backgroundImage.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeInTime;
            backgroundImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        backgroundImage.color = tempColor;
        if (fadeInCallBack != null)
            fadeInCallBack();

        fadeInCallBack = null;
        fadeInCoroutine = null;
    }

    IEnumerator IFadeOutEffect(float _fadeOutTime)
    {
        if (fadeOutCallBack != null)
            fadeOutCallBack();

        Color tempColor = backgroundImage.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeOutTime;
            backgroundImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        backgroundImage.color = tempColor;
        
        fadeOutCallBack = null;
        fadeOutCoroutine = null;

        gameObject.SetActive(false);
    }
}
