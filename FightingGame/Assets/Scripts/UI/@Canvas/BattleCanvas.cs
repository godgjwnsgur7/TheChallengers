using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI blueTeamStatusWindow;
    [SerializeField] StatusWindowUI redTeamStatusWindow;
    [SerializeField] ResultWindowUI resultWindow;
    [SerializeField] GameStateEffectUI gameStateEffect;
    [SerializeField] TimerUI timer;
    [SerializeField] FightingInfoWindow FightingInfoWindow;

    public StatusWindowUI Get_StatusWindowUI(ENUM_TEAM_TYPE _teamType)
    {
        if (_teamType == ENUM_TEAM_TYPE.Blue)
            return blueTeamStatusWindow;
        else if (_teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindow;
        else
        {
            Debug.Log($"_teamType 오류 : {_teamType}");
            return null;
        }
    }

    public void Register_TimerCallBack()
    {
        timer.Register_TimerCallBack();
    }

    public void Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE effectType)
    {
        gameStateEffect.Play_GameStateEffect(effectType);
    }

    public void EndGame(bool isDraw, bool isWin) => resultWindow.Open(isDraw, isWin);
}