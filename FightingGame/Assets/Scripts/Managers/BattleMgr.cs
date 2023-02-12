using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 배틀 씬에서의 게임 시작 전 후의 임시데이터를 임시저장
/// 게임의 시작과 끝을 판단
/// </summary>
public class BattleMgr
{
    public bool isGamePlayingState
    {
        private set;
        get;
    }

    ActiveCharacter enemyCharacter;

    public void Init()
    {
        isGamePlayingState = false;
    }

    public void Clear()
    {

    }

    public void GameStart()
    {
        isGamePlayingState = true;

        if(PhotonLogicHandler.IsMasterClient)
        {
            Managers.Network.Start_Timer();
        }
    }

    public void EndGame(ENUM_TEAM_TYPE _losingTeam)
    {
        isGamePlayingState = false;

        BattleCanvas battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();

        bool isWin = (_losingTeam != Managers.Player.Get_TeamType());
        
        bool isDraw = (isWin ? Managers.Player.Get_PlayerState() == ENUM_PLAYER_STATE.Die
            : enemyCharacter.currState == ENUM_PLAYER_STATE.Die);

        if (isDraw)
            battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.DrawTrigger);
        else
        {
            if (isWin)
                battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.WinTrigger);
            else
                battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.LoseTrigger);
        }
    }

    public void Char_ReferenceRegistration(ActiveCharacter _enemyCharacter)
    {
        enemyCharacter = _enemyCharacter;
    }
}