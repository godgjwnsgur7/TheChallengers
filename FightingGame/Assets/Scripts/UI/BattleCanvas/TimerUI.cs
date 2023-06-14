using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerUI : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] Text pingText;

    bool isTimeLimitSound = false;

    public void Register_TimerCallBack()
    {
        isTimeLimitSound = false;

        Update_Timer((int)Managers.Data.gameInfo.maxGameRunTime); // 초기세팅

        Managers.Network.Register_TimerCallBack(Update_Timer);
    }

    /// <summary>
    /// UserSyncMediator에 등록될 CallBack함수
    /// </summary>
    public void Update_Timer(int timeLimit)
    {
        int seconds = timeLimit;
        int minutes = 0;

        while (seconds >= 60.0f)
        {
            seconds -= 60;
            minutes++;
        }

        if(isTimeLimitSound == false && timeLimit <= 30)
        {
            isTimeLimitSound = true;
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_TimeLimit);
        }

        Set_TimerText(minutes, seconds);
    }

    private void Set_TimerText(int minutes, int seconds)
    {
        timerText.text = String.Format("{0:0}:{1:00}", minutes, seconds);
        pingText.text = $"Ping : {PhotonLogicHandler.Ping}";
    }
}