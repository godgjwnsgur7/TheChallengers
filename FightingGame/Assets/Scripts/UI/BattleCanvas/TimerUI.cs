using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerUI : MonoBehaviour
{
    [SerializeField] Text timerText;

    Coroutine timerCoroutine;
    Action timeOutCallBack;

    bool isTimerLock = false;

    public void Start_Timer(Action _timeOutCallBack)
    {
        timeOutCallBack = _timeOutCallBack;

        isTimerLock = false; 
        timerCoroutine = StartCoroutine(IStartTimer());
    }

    public void Stop_Timer()
    {
        StopCoroutine(timerCoroutine);
        isTimerLock = true;
    }

    protected IEnumerator IStartTimer()
    {
        /*
        float limitTime = 240.0f; // 일단 게임시간은 4분으로 고정 (임시)
        float currLimitImte;
        float elapsedTime;
        int minute;
        */

        while (!isTimerLock)
        {
            /* 방식을 바꿔야 함
            elapsedTime = Time.time - startTime;
            currLimitImte = limitTime - elapsedTime;

            minute = (int)Math.Floor(currLimitImte / 60f);
            currLimitImte -= minute * 60;

            timerText.text = string.Format("{0:00}:{1:00}", minute, currLimitImte);
            */

            yield return null;
        }
    }
}