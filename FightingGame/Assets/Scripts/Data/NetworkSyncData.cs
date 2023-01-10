using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 마스터 클라이언트만 제어가 가능할 것 - 배틀 씬에서만 사용
/// </summary>
public class NetworkSyncData : MonoBehaviourPhoton
{
    Action<float> updateTimerCallBack = null;

    Coroutine timerCoroutine = null;

    float gameRunTimeLimit = 270.0f; // 게임 시간은 4분 30초로 고정

    bool isInitialized = false;

    public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log("중복으로 네트워크싱크데이터를 초기화 시도하였습니다.");
            return;
        }

        isInitialized = true;

        base.Init();

        Managers.Battle.Set_NetworkSyncData(this);
        Request_ConnectTimerCallBack();

        if (PhotonLogicHandler.IsMasterClient)
        {
            StartCoroutine(IGameStartDelayTimeCheck(1.0f));
        }
    }

    public void Clear()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        updateTimerCallBack = null;
    }

    #region Sync Variable
    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        stream.Write(gameRunTimeLimit);

        base.OnMineSerializeView(stream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        gameRunTimeLimit = stream.Read<float>();

        base.OnOtherSerializeView(stream);
    }
    #endregion

    public void Request_ConnectTimerCallBack() => Managers.UI.currCanvas.GetComponent<BattleCanvas>().Register_TimerCallBack();
    public void Set_TimerCallBack(Action<float> _updateTimerCallBack) => updateTimerCallBack = _updateTimerCallBack;

    [BroadcastMethod]
    public void Ready_Game()
    {
        Managers.Battle.GameReady();
    }

    [BroadcastMethod]
    public void Start_Game()
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        timerCoroutine = StartCoroutine(IStartTimer());
    }

    [BroadcastMethod]
    public void TimeOver_EndGame()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        Managers.Battle.TimeOver();
    }

    [BroadcastMethod]
    public void Sync_TimerCallBack(float _gameTimeLimit)
    {
        updateTimerCallBack(_gameTimeLimit);
    }

    protected IEnumerator IGameStartDelayTimeCheck(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Managers.Battle.Sync_GameReady();
    }

    protected IEnumerator IStartTimer()
    {
        while (gameRunTimeLimit >= 0.1f)
        {
            gameRunTimeLimit -= Time.deltaTime;

            PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData, float>
                (this, Sync_TimerCallBack, gameRunTimeLimit);

            yield return null;
        }

        if(gameRunTimeLimit < 0.1f)
        {
            // 타임아웃 게임종료
            PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData, float>
                    (this, Sync_TimerCallBack, 0.0f);
            
            PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData>(this, TimeOver_EndGame);
        }

        timerCoroutine = null;
    }
}
