using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

/// <summary>
/// 네트워크매니저가 브로드캐스트함수를 사용하기 위해 존재
/// </summary>
public class UserSyncMediator : MonoBehaviourPhoton
{
    Action<int> updateTimerCallBack = null;
    Coroutine timerCoroutine = null;

    int gameRunTimeLimit = 0;
    bool isInit = false;

    private void Awake()
    {
        Managers.Network.Set_UserSyncMediator(this);
    }

    public override void OnDisable()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        base.OnDisable();
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

        if (gameRunTimeLimit <= 0.1f)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator, int>(this, Sync_TimerCallBack, 0);

            // 타임아웃으로 게임 종료 (체력으로 양쪽 게임 판단)
            PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, Sync_EndGameTimeOut);
        }
    }

    [BroadcastMethod]
    public void Sync_EndGameTimeOut()
    {
        Managers.Battle.EndGame_TimeOut();
    }

    [BroadcastMethod]
    public void Sync_TimerCallBack(int _currentTimeLimit)
    {
        updateTimerCallBack(_currentTimeLimit);
    }

    public void SyncPlaySFX_HitSound(int hitSoundTypeNum, ENUM_TEAM_TYPE teamType, Vector3 hitPosVec)
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator, int, ENUM_TEAM_TYPE, Vector3>
                   (this, PlaySFX_HitSound, hitSoundTypeNum, teamType, hitPosVec);
    }

    [BroadcastMethod]
    public void PlaySFX_HitSound(int hitSoundTypeNum, ENUM_TEAM_TYPE teamType, Vector3 hitPosVec)
    {
        Managers.Sound.Play_SFX((ENUM_SFX_TYPE)hitSoundTypeNum, teamType, hitPosVec);
    }

    /// <summary>
    ///  게임에 돌입하기 전에 처리되는 함수들
    /// </summary>
    public void Sync_ShowGameInfo()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, Show_GameInfoWindow);
    }

    [BroadcastMethod]
    public void Show_GameInfoWindow()
    {
        Managers.Sound.Stop_BGM();
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_GameStartWindow();
    }
    
    public void Sync_ShowGameStartInfo()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, ShowGameStartInfo);
    }
    [BroadcastMethod]
    public void ShowGameStartInfo()
    {
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().GameStart();
    }

    /// <summary>
    /// Photon의 커스텀 플레이어 프로퍼티 테이블의 데이터 값을 초기 값으로 되돌림
    /// </summary>
    public void Sync_RequestUnSyncDataAll()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, Request_UnSyncDataAll);
    }
    [BroadcastMethod]
    public void Request_UnSyncDataAll()
    {
        PhotonLogicHandler.Instance.RequestUnSyncDataAll();
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