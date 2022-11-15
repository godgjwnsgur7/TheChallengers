using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerNotifyPopup : PopupUI
{
    [SerializeField] Text popupText;

    Action timeOutCallBack;

    Coroutine timeOutCheckCoroutine;

    public void Open(string _message, float _runTime = 2.0f, Action _timeOutCallBack = null)
    {
        popupText.text = _message;
        timeOutCallBack = _timeOutCallBack;

        this.gameObject.SetActive(true);

        timeOutCheckCoroutine = StartCoroutine(ICloseTimeCheck(_runTime));
    }
    public void Open_Again(string _message, float _runTime = 2.0f, Action _timeOutCallBack = null)
    {
        Close();

        Open(_message, _runTime, _timeOutCallBack);
    }

    private void Close()
    {
        this.gameObject.SetActive(false);

        popupText.text = null;
        if (timeOutCheckCoroutine != null)
            StopCoroutine(timeOutCheckCoroutine);

        if (timeOutCallBack != null)
            timeOutCallBack();

        timeOutCallBack = null;
    }

    protected IEnumerator ICloseTimeCheck(float runTime)
    {
        yield return new WaitForSeconds(runTime);
        Close();

        timeOutCheckCoroutine = null;
    }

}
