using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

/// <summary>
/// 네트워크매니저가 브로드캐스트멀티를 사용하기 위해 존재
/// </summary>
public class UserSyncMediator : MonoBehaviourPhoton
{
    Action<int> updateTimerCallBack = null;
    Coroutine timerCoroutine = null;

    int gameRunTimeLimit = 0;

    bool isInit = false;

    public override void OnDisable()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    public override void Init()
    {
        if (isInit)
        {
            Debug.LogError("중복으로 UserSyncMediator를 초기화 시도하였습니다.");
            return;
        }

        base.Init();

        DontDestroyOnLoad(gameObject);
        Managers.Network.Set_UserSyncMediator(this);
        gameRunTimeLimit = (int)Managers.Data.gameInfo.maxGameRunTime;
    }

    public void Register_TimerCallBack(Action<int> _updateTimerCallBack)
    {
        updateTimerCallBack = _updateTimerCallBack;
    }

    public void Start_Timer()
    {
        timerCoroutine = StartCoroutine(IStartTimer());
    }

    protected IEnumerator IStartTimer()
    {
        float currentTimerLimit = gameRunTimeLimit;

        while (gameRunTimeLimit >= 0.1f && Managers.Battle.isGamePlayingState)
        {
            currentTimerLimit -= Time.deltaTime;

            if ((int)currentTimerLimit != gameRunTimeLimit)
            {
                gameRunTimeLimit = (int)currentTimerLimit;

                PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator, int>
                    (this, Sync_TimerCallBack, gameRunTimeLimit);
            }

            yield return null;
        }

        if (gameRunTimeLimit >= 0.1f)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator, int>(this, Sync_TimerCallBack, 0);
            // 타임아웃으로 게임 종료된 것
        }
    }

    [BroadcastMethod]
    public void Sync_TimerCallBack(int _currentTimeLimit)
    {
        updateTimerCallBack(_currentTimeLimit);
    }
    
    /// <summary>
    ///  게임에 돌입하기 전에 처리되는 함수들
    /// </summary>
    public void Sync_ShowGameInfo()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, ShowGameInfo);
    }
    [BroadcastMethod]
    public void ShowGameInfo()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(Open_FightingGameInfo);
    }
    public void Open_FightingGameInfo()
    {
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_FightingInfoWindow();
    }

    /// <summary>
    /// 배틀 씬에 돌입이 끝난 후에 처리되는 함수들
    /// </summary>
    public void Sync_GameStartEffect()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, GameStartEffect);
    }
    [BroadcastMethod]
    public void GameStartEffect()
    {
        Managers.UI.currCanvas.GetComponent<BattleCanvas>().Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.GameStartTrigger);
    }
}