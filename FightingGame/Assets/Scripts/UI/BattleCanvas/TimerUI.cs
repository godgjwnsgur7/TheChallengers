using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerUI : MonoBehaviourPhoton
{
    [SerializeField] Text timerText;

    Coroutine timerCoroutine;
    Action timeOutCallBack;

    bool isTimerLock = false;

    // 우려되는 예외상황 : 핸드폰 내의 날짜와 시간으로 측정될탠데, (임시)
    // 만약 핸드폰 내의 시간이 한국 시간으로 되어있지 않는, 등의 예외상황에서는?..

    [BroadcastMethod]
    public void Start_Timer(Action _timeOutCallBack, float startTime)
    {
        Init();

        timeOutCallBack = _timeOutCallBack;

        isTimerLock = false; 
        timerCoroutine = StartCoroutine(IStartTimer(startTime));
    }

    public void Stop_Timer()
    {
        StopCoroutine(timerCoroutine);
        isTimerLock = true;
    }

    protected IEnumerator IStartTimer(float startTime)
    {
        float limitTime = 240.0f; // 일단 게임시간은 4분으로 고정 (임시)
        float currLimitImte;
        float elapsedTime;
        int minute;

        while (!isTimerLock)
        {
            elapsedTime = Time.time - startTime;
            currLimitImte = limitTime - elapsedTime;

            minute = (int)Math.Floor(currLimitImte / 60f);
            currLimitImte -= minute * 60;

            timerText.text = string.Format("{0:00}:{1:00}", minute, currLimitImte);

            yield return null;
        }
    }
}