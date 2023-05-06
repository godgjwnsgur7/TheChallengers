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
    } = false;

    ActiveCharacter enemyCharacter;

    public void Clear()
    {
		PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnLeftGame;
	}

    public void GameStart()
    {
        isGamePlayingState = true;

        if(PhotonLogicHandler.IsMasterClient)
        {
            Managers.Network.Start_Timer();
        }

		PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnLeftGame;
		PhotonLogicHandler.Instance.onLeftRoomPlayer += OnLeftGame;
	}

    private void OnLeftGame(string enemyNickname)
    {
		if (PhotonLogicHandler.CurrentMyNickname != enemyNickname) // 상대방 이탈
		{
			var team = Managers.Player.Get_TeamType();

			int enemyTeamInt = ((int)team * 2) % (int)ENUM_TEAM_TYPE.Max;
			team = (ENUM_TEAM_TYPE)enemyTeamInt;

			EndGame(team);
		}

		PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnLeftGame;
	}

	public void EndGame(ENUM_TEAM_TYPE _losingTeam)
    {
        if (isGamePlayingState == false)
            return;

        isGamePlayingState = false;

        BattleCanvas battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();

        bool isWin = _losingTeam != Managers.Player.Get_TeamType();
        
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

    public void EndGame_TimeOut()
    {
        if (isGamePlayingState == false)
            return;

        isGamePlayingState = false;

        BattleCanvas battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();
        float blueTeamHPFillAmount = battleCanvas.Get_CurrHPFillAmount(ENUM_TEAM_TYPE.Blue);
        float redTeamHPFillAmount = battleCanvas.Get_CurrHPFillAmount(ENUM_TEAM_TYPE.Red);

        // 무승부
        if (blueTeamHPFillAmount == redTeamHPFillAmount)
        {
            battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.DrawTrigger);
            return;
        }

        // 이긴 팀 체크
        ENUM_TEAM_TYPE WinnerTeam = (blueTeamHPFillAmount > redTeamHPFillAmount) ? ENUM_TEAM_TYPE.Blue : ENUM_TEAM_TYPE.Red;

        if (WinnerTeam == Managers.Player.Get_TeamType())
            battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.WinTrigger);
        else
            battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.LoseTrigger);
    }

    public void Char_ReferenceRegistration(ActiveCharacter _enemyCharacter)
    {
        enemyCharacter = _enemyCharacter;
    }
}