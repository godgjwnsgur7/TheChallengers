using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI buleTeamStatusWindowUI;
    [SerializeField] StatusWindowUI redTeamStatusWindowUI;
    [SerializeField] TimerUI timerUI;
    
    public override void Open<T>(UIParam param = null)  
    {
        if (typeof(T) == typeof(TimerUI)) timerUI.Open(param);
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(TimerUI)) timerUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public override T GetUIComponent<T>()
    {

        return default(T);
    }

    public StatusWindowUI Get_StatusWindowUI(ENUM_TEAM_TYPE _teamType)
    {
        if(_teamType == ENUM_TEAM_TYPE.Blue)
            return buleTeamStatusWindowUI;
        else if(_teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindowUI;
        else
        {
            Debug.Log($"_teamType 오류 : {_teamType}");
            return null;
        }
    }

}