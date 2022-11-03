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

    float gameTimeLimit = 270.0f; // 게임 시간은 4분 30초로 고정

    bool isInitialized = false;
    // 얘로 로드된 시점 받아와도 괜찮을 거 같은데, 일단 보류

    public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log("중복으로 네트워크싱크데이터를 초기화 시도하였습니다.");
            return;
        }

        isInitialized = true;

        base.Init();
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
        stream.Write(gameTimeLimit);

        base.OnMineSerializeView(stream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        gameTimeLimit = stream.Read<float>();

        base.OnOtherSerializeView(stream);
    }
    #endregion

    [BroadcastMethod]
    public void Request_ConnectTimerCallBack() => Managers.UI.currCanvas.GetComponent<BattleCanvas>().Register_TimerCallBack();

    /// <summary>
    /// 슬레이브 클라이언트에서 호출될 함수
    /// </summary>
    [BroadcastMethod]
    public void Connect_BattleMgr()
    {
        Managers.Battle.Get_NetworkSyncData(this);
    }

    public void Connect_TimerCallBack(Action<float> _updateTimerCallBack)
    {
        updateTimerCallBack = _updateTimerCallBack;
    }

    [BroadcastMethod]
    public void Start_GameTimer()
    {
        timerCoroutine = StartCoroutine(IStartTimer());
    }

    [BroadcastMethod]
    public void Stop_GameTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    [BroadcastMethod]
    public void Sync_TimerCallBack(float _gameTimeLimit)
    {
        updateTimerCallBack(_gameTimeLimit);
    }

    protected IEnumerator IStartTimer()
    {
        while(gameTimeLimit >= 0.1f)
        {
            gameTimeLimit -= Time.deltaTime;

            PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData, float>
                (this, Sync_TimerCallBack, gameTimeLimit);

            yield return null;
        }

        if(gameTimeLimit < 0.1f)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData, float>
                    (this, Sync_TimerCallBack, 0.0f);
        }

        // 게임 종료 go.
        PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData>
                    (this, Stop_GameTimer);
    }
}
