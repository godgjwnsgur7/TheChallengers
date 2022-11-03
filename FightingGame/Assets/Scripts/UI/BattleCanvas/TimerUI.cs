using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerUI : MonoBehaviour
{
    [SerializeField] Text timerText;

    public void Register_TimerCallBack()
    {
        Managers.Battle.Connect_TimerCallBack(Update_Timer);
    }

    /// <summary>
    /// NetworkSyncData에 등록될 CallBack함수
    /// </summary>
    public void Update_Timer(float timeLimit)
    {
        int seconds = (int)timeLimit;
        int minutes = 0;

        while (seconds >= 60.0f)
        {
            seconds -= 60;
            minutes++;
        }

        Set_TimerText(minutes, seconds);
    }

    private void Set_TimerText(int minutes, int seconds)
    {
        timerText.text = String.Format("{0:0}:{1:00}", minutes, seconds);
    }
}