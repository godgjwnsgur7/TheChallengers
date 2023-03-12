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
    Action fadeOutInCallBack = null;

    Coroutine fadeInCoroutine = null;
    Coroutine fadeOutCoroutine = null;
    Coroutine fadeOutInCoroutine = null;

    float fadeEffectRunTime = 0.5f;

    /// <summary>
    /// 서서히 검은 화면이 사라짐
    /// </summary>
    public void Play_FadeInEffect(Action _fadeInCallBack = null)
    {
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);
        if (fadeOutCoroutine != null)
            StopCoroutine(fadeOutCoroutine);

        fadeInCallBack = _fadeInCallBack;

        backgroundImage.color = new Color(0, 0, 0, 1);

        fadeInCoroutine = StartCoroutine(IFadeInEffect(fadeEffectRunTime));
    }

    /// <summary>
    /// 서서히 검은 화면이 됨
    /// </summary>
    public void Play_FadeOutEffect(Action _fadeOutCallBack = null)
    {
        if (fadeOutInCoroutine != null)
            StopCoroutine(fadeOutInCoroutine);
        if (fadeInCoroutine != null)
            StopCoroutine(fadeInCoroutine);

        fadeOutCallBack = _fadeOutCallBack;

        backgroundImage.color = new Color(0, 0, 0, 0);

        this.gameObject.SetActive(true);

        fadeOutCoroutine = StartCoroutine(IFadeOutEffect(fadeEffectRunTime));
    }

    public void Play_FadeOutInEffect(Action _fadeOutInCallBack = null)
    {
        Stop_Coroutine(fadeInCoroutine);
        Stop_Coroutine(fadeOutCoroutine);

        fadeOutInCallBack = _fadeOutInCallBack;

        backgroundImage.color = new Color(0, 0, 0, 0);

        this.gameObject.SetActive(true);

        fadeOutInCoroutine = StartCoroutine(IFadeOutInEffect(fadeEffectRunTime));
    }

    IEnumerator IFadeOutEffect(float _fadeOutTime)
    {
        Stop_Coroutine(fadeOutInCoroutine);
        Stop_Coroutine(fadeInCoroutine);

        Color tempColor = backgroundImage.color;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeOutTime;
            backgroundImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        backgroundImage.color = tempColor;

        fadeOutCallBack?.Invoke();

        fadeOutCallBack = null;
        fadeOutCoroutine = null;
    }

    IEnumerator IFadeInEffect(float _fadeInTime)
    {
        Stop_Coroutine(fadeOutCoroutine);
        Stop_Coroutine(fadeOutInCoroutine);

        fadeInCallBack?.Invoke();

        Color tempColor = backgroundImage.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeInTime;
            backgroundImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        backgroundImage.color = tempColor;

        fadeInCallBack = null;
        fadeInCoroutine = null;

        gameObject.SetActive(false);
    }

    IEnumerator IFadeOutInEffect(float _fadeInOutTime)
    {
        Color tempColor = backgroundImage.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeInOutTime;
            backgroundImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        backgroundImage.color = tempColor;

        fadeOutInCallBack?.Invoke();

        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeInOutTime;
            backgroundImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        backgroundImage.color = tempColor;

        fadeOutInCallBack = null;
        fadeOutInCoroutine = null;

        gameObject.SetActive(false);
    }

    private void Stop_Coroutine(Coroutine _coroutine)
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }
}
