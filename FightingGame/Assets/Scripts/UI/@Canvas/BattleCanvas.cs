using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI buleTeamStatusWindow;
    [SerializeField] StatusWindowUI redTeamStatusWindow;
    [SerializeField] ResultWindowUI resultWindow;
    [SerializeField] TimerUI timer;
    public StatusWindowUI Get_StatusWindowUI(ENUM_TEAM_TYPE _teamType)
    {
        if(_teamType == ENUM_TEAM_TYPE.Blue)
            return buleTeamStatusWindow;
        else if(_teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindow;
        else
        {
            Debug.Log($"_teamType 오류 : {_teamType}");
            return null;
        }
    }

    public void StartGame()
    {
        timer.Start_Timer(EndGame);
    }

    public void EndGame()
    {
        
    }

}