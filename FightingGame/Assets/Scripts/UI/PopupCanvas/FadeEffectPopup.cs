using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeEffectPopup : PopupUI
{
    [SerializeField] Image backgroundImage;

    Coroutine fadeInCoroutine = null;
    Coroutine fadeOutCoroutine = null;
    Coroutine fadeOutInCoroutine = null;

    float fadeEffectRunTime = 0.5f;

    protected override void OnEnable()
    {
        Managers.UI.IsExitKeyLock(true);
    }

    protected override void OnDisable()
    {
        Managers.UI.IsExitKeyLock(false);
    }

    public bool Get_FadeState()
    {
        bool isFadeState = backgroundImage.gameObject.activeSelf && (backgroundImage.color.a == 1f);
        // Debug.Log($"isFadeState : {isFadeState}");
        return isFadeState;
    }

    /// <summary>
    /// 서서히 검은 화면이 사라짐
    /// </summary>
    public void Play_FadeInEffect(Action _fadeInCallBack = null)
    {
        // 페이드인 중이면 리턴
        if (fadeInCoroutine != null || fadeOutInCoroutine != null)
        {
            Debug.Log("페이드인 중복 실행");
            _fadeInCallBack?.Invoke();
            return;
        }

        fadeInCoroutine = CoroutineHelper.StartCoroutine(IFadeInEffect(_fadeInCallBack, fadeEffectRunTime));
    }

    /// <summary>
    /// 서서히 검은 화면이 됨
    /// </summary>
    public void Play_FadeOutEffect(Action _fadeOutCallBack = null)
    {
        if (fadeOutInCoroutine != null)
        {
            Debug.Log("페이드아웃 중복 실행");
            _fadeOutCallBack?.Invoke();
            return;
        }

        fadeOutCoroutine = CoroutineHelper.StartCoroutine(IFadeOutEffect(_fadeOutCallBack, fadeEffectRunTime));
    }

    public void Play_FadeOutInEffect(Action _fadeOutInCallBack = null)
    {
        if (fadeOutInCoroutine != null)
        {
            Debug.Log("페이드아웃인 중복 실행");
            _fadeOutInCallBack?.Invoke();
            return;
        }

        fadeOutInCoroutine = CoroutineHelper.StartCoroutine(IFadeOutInEffect(_fadeOutInCallBack, fadeEffectRunTime));
    }

    IEnumerator IFadeOutEffect(Action _fadeOutCallBack, float _fadeOutTime)
    {
        if(gameObject.activeSelf)
        {
            _fadeOutCallBack?.Invoke();
            fadeOutCoroutine = null;
            yield break;
        }

        yield return new WaitUntil(() => (fadeOutInCoroutine == null) && (fadeOutCoroutine == null));

        backgroundImage.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(true);
        Color tempColor = backgroundImage.color;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeOutTime;
            backgroundImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        backgroundImage.color = tempColor;
        Managers.UI.popupCanvas.DeactivePopupAll();
        _fadeOutCallBack?.Invoke();
        fadeOutCoroutine = null;
    }

    IEnumerator IFadeInEffect(Action _fadeInCallBack, float _fadeInTime)
    {
        yield return new WaitUntil(() => fadeOutCoroutine == null);

        _fadeInCallBack?.Invoke();

        backgroundImage.color = new Color(0, 0, 0, 1);
        Color tempColor = backgroundImage.color;

        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeInTime;
            backgroundImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        backgroundImage.color = tempColor;
        fadeInCoroutine = null;
        gameObject.SetActive(false);
    }

    IEnumerator IFadeOutInEffect(Action _fadeOutInCallBack, float _fadeOutInTime)
    {
        yield return new WaitUntil(() => (fadeOutInCoroutine == null) && (fadeOutInCoroutine == null));

        backgroundImage.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(true);
        Color tempColor = backgroundImage.color;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / _fadeOutInTime;
            backgroundImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        backgroundImage.color = tempColor;

        Managers.UI.popupCanvas.DeactivePopupAll();

        _fadeOutInCallBack?.Invoke();

        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / _fadeOutInTime;
            backgroundImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        backgroundImage.color = tempColor;
        fadeOutInCoroutine = null;
        gameObject.SetActive(false);
    }
}
