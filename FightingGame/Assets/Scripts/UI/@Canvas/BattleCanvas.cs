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

    public StatusWindowUI Get_StatusWindowUI(ENUM_TEAM_TYPE teamType)
    {
        if(teamType == ENUM_TEAM_TYPE.Blue)
            return buleTeamStatusWindowUI;
        else if(teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindowUI;
        else
        {
            Debug.Log("알 수 없는 팀");
            return null;
        }
    }

}