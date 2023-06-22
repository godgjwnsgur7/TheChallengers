using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI blueTeamStatusWindow;
    [SerializeField] StatusWindowUI redTeamStatusWindow;
    [SerializeField] ResultWindowUI resultWindow;
    [SerializeField] GameStateEffectUI gameStateEffect;
    [SerializeField] TimerUI timer;

    public override void Init()
    {
        base.Init();
        
        if(PhotonLogicHandler.IsConnected)
            Register_TimerCallBack();
    }

    public Action<float> Get_StatusWindowCallBack(ENUM_TEAM_TYPE _teamType, ENUM_CHARACTER_TYPE _charType)
    {
        if (_teamType == ENUM_TEAM_TYPE.Blue)
            return blueTeamStatusWindow.Connect_Character(_charType);
        else if (_teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindow.Connect_Character(_charType);
        else
        {
            Debug.Log($"_teamType 오류 : {_teamType}");
            return null;
        }
    }

    public float Get_CurrHPFillAmount(ENUM_TEAM_TYPE tempType)
    {
        if(tempType == ENUM_TEAM_TYPE.Blue)
            return blueTeamStatusWindow.Get_CurrHPFillAmount();
        else if(tempType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindow.Get_CurrHPFillAmount();

        return 0;
    }

    public void Register_TimerCallBack()
    {
        timer.Register_TimerCallBack();
    }

    public void Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE effectType)
    {
        gameStateEffect.Play_GameStateEffect(effectType);
    }

    public void EndGame(bool isDraw, bool isWin)
    {
        Managers.Sound.Stop_BGM();
        resultWindow.Open(isDraw, isWin);
    }

    public override void OnClick_Exit()
    {
        // (점수깎아)
        Managers.UI.popupCanvas.Open_SelectPopup(() => { Application.Quit(); }
        , null, "게임을 종료하시겠습니까?\n진행중인 게임은 패배처리 됩니다.");
    }
}