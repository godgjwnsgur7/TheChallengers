using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI statusWindowUI;
    [SerializeField] TimerUI timerUI;
    
    public override void Open<T>(UIParam param = null)  
    {
        if (typeof(T) == typeof(StatusWindowUI)) statusWindowUI.Open(param);
        else if (typeof(T) == typeof(TimerUI)) timerUI.Open(param);
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(StatusWindowUI)) statusWindowUI.Close();
        else if (typeof(T) == typeof(TimerUI)) timerUI.Close();
        else Debug.Log("범위 벗어남");
    }
}