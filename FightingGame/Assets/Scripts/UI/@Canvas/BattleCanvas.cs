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
    
    public override T GetUIComponent<T>()
    {
        // 이거 되나? ㅋㅋㅋ 테스트 안함 아직 (임시)
        if (typeof(T) == typeof(ResultWindowUI)) return resultWindow.GetComponent<T>();
        return default(T);
    }

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

}