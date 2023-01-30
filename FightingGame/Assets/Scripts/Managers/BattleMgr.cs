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

    string slaveNickname = null;
   

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public void GameStart()
    {
        isGamePlayingState = true;
        // 게임 시작도 얘가 알림받고 있어.
        // 이 시점은, 해당하는 이펙트가 끝났으니 이제 캐릭터의 락을 풀어도 된단 의미가 됨
        // 하지만, 그건 얘가 브로드캐스트로 수행해야 하는 부분인데 음....?
    }

    public void EndGame(ENUM_TEAM_TYPE _losingTeam)
    {
        // 게임 종료를 얘가 알림받고 있음 일단...

        /*
        isGamePlayingState = false;
        
        if (battleCanvas == null)
            battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();

        bool isWin = (losingTeam != activeCharacter.teamType);
        bool isDraw = (isWin ? activeCharacter.currState == ENUM_PLAYER_STATE.Die
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
        */
    }

    public void Set_SlaveNickname(string _slaveNickname)
    {
        this.slaveNickname = _slaveNickname;
    }

    public string Get_SlaveNickname() => slaveNickname;
}