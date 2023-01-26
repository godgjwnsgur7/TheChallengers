using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 네트워크매니저가 브로드캐스트멀티를 사용하기 위해 존재
/// </summary>
public class UserSyncMediator : MonoBehaviourPhoton
{
    Action<int> updateTimerCallBack = null;
    Action showGameInfoCallBack = null;
    Coroutine timerCoroutine = null;

    float gameRunTimeLimit;

    bool isInit = false;

    public override void OnDisable()
    {
        if(timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    public override void Init()
    {
        if(isInit)
        {
            Debug.LogError("중복으로 UserSyncMediator를 초기화 시도하였습니다.");
            return;
        }

        base.Init();

        DontDestroyOnLoad(gameObject);        
        Managers.Network.Set_UserSyncMediator(this);
    }

    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        // stream.Write(isMasterClearComplete);

        base.OnMineSerializeView(stream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        // isMasterClearComplete = stream.Read<bool>();

        base.OnOtherSerializeView(stream);
    }

    public void Register_GameInfoCallBack(Action _showGameInfoCallBack)
    {
        showGameInfoCallBack = _showGameInfoCallBack;
    }

    public void Register_TimerCallBack(Action<int> _updateTimerCallBack)
    {
        updateTimerCallBack = _updateTimerCallBack;
    }

    protected IEnumerator IStartTimer()
    {
        gameRunTimeLimit = Managers.Data.gameInfo.maxGameRunTime;

        int currentTimerLimit = (int)gameRunTimeLimit;

        while (gameRunTimeLimit >= 0.1f)
        {
            gameRunTimeLimit -= Time.deltaTime;

            if(currentTimerLimit != (int)gameRunTimeLimit)
            {
                currentTimerLimit = (int)gameRunTimeLimit;
                PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator, int>
                    (this, Sync_TimerCallBack, currentTimerLimit);
            }

            yield return null;
        }
    }

    [BroadcastMethod]
    public void Sync_TimerCallBack(int _currentTimeLimit)
    {
        updateTimerCallBack(_currentTimeLimit);
    }

    public void Show_GameInfo()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<UserSyncMediator>(this, Sync_ShowGameInfo);
    }
    
    [BroadcastMethod]
    public void Sync_ShowGameInfo()
    {
        // FadeIn은 FightingGameInfo에서 정보를 받아온 후에 실행
        Managers.UI.popupCanvas.Play_FadeOutEffect(showGameInfoCallBack);
    }
}