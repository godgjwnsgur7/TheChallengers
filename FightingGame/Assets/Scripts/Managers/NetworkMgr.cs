using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// Ready, DataSync, SceneSync, CharacterSync
/// </summary>
public class SyncData
{
    public bool isDataSync;
    public bool isReady;
    public bool isSceneSync;
    public bool isCharacterSync;
    public ENUM_CHARACTER_TYPE characterType;

    public SyncData(bool _isDataSync, bool _isReady, bool _isSceneSync, bool _isCharacterSync)
    {
        isDataSync = _isDataSync;
        isReady = _isReady;
        isSceneSync = _isSceneSync;
        isCharacterSync = _isCharacterSync;
    }
}

/// <summary>
/// 게임의 전반적인 관리를 할 매니저
/// 서버접속상태 체크, 네트워크 끊김 등을 판단
/// </summary>
public class NetworkMgr : IRoomPostProcess
{
    UserSyncMediator userSyncMediator = null;

    SyncData masterSyncData = null;
    SyncData slaveSyncData = null;

    Coroutine sequenceExecuteCoroutine = null;
    
    public void Init()
    {
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= OnEnterRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += OnExitRoomCallBack;

        PhotonLogicHandler.Instance.onEnterRoomPlayer -= OnEnterRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += OnExitRoomCallBack;

        this.RegisterRoomCallback();
    }

    public void Set_UserSyncMediator(UserSyncMediator _userSyncMediator)
    {
        if (userSyncMediator != null && userSyncMediator.gameObject)
            Managers.Resource.Destroy(userSyncMediator.gameObject);

        userSyncMediator = _userSyncMediator;
    }

    public void OnEnterRoomCallBack(string enterUserNickname)
    {
        if (PhotonLogicHandler.CurrentMyNickname == enterUserNickname)
        {
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
        }
        else // 상대가 입장
        {
            if(PhotonLogicHandler.IsMasterClient)
            {
                sequenceExecuteCoroutine = CoroutineHelper.StartCoroutine(INetworkSequenceExecuter());
            }
        }
    }

    public void OnExitRoomCallBack(string exitUserNickname)
    {
        Managers.Resource.Destroy(userSyncMediator.gameObject);

        // 나간 유저가 내가 아니라면 (내가 원래 마스터클라이언트였다면)
        if (PhotonLogicHandler.CurrentMyNickname != exitUserNickname)
        {
            CoroutineHelper.StopCoroutine(sequenceExecuteCoroutine);
            sequenceExecuteCoroutine = CoroutineHelper.StartCoroutine(INetworkSequenceExecuter());
        }
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {

    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        SyncData _syncData = new SyncData(property.isReady, property.isDataSync, property.isSceneSync, property.isCharacterSync);
    
        if(property.isMasterClient)
            masterSyncData = _syncData;
        else
            slaveSyncData = _syncData;
    }

    public void Register_GameInfoCallBack(Action _showGameInfoCallBack)
    {
        userSyncMediator.Register_GameInfoCallBack(_showGameInfoCallBack);
    }

    public void Register_TimerCallBack(Action<int> _updateTimerCallBack)
    {
        userSyncMediator.Register_TimerCallBack(_updateTimerCallBack);
    }

    public void Clear()
    {
        this.UnregisterRoomCallback();
    }

    protected IEnumerator INetworkSequenceExecuter()
    {
        if (!PhotonLogicHandler.IsMasterClient)
        {
            sequenceExecuteCoroutine = null;
            yield break;
        }

        // 1. 연결 확인
        yield return new WaitUntil(Get_DataSyncAllState);
        // 양쪽 다 연결되면 유저싱크메디에이터를 생성함
        Managers.Resource.InstantiateEveryone("UserSyncMediator");

        // 2. 레디 확인 (마스터의 레디 == 시작 : 레디조건이 슬레이브의 준비완료가 될 것)
        yield return new WaitUntil(Get_ReadyAllState);
        // 둘다 레디를 확인하면 게임 돌입을 알림
        PhotonLogicHandler.Instance.OnGameStart();
        // 게임시작정보를 알려줌 (씬 로드는 이쪽에서 처리함)
        userSyncMediator.Show_GameInfo();

        // 4. 씬 로드 확인
        yield return new WaitUntil(Get_SceneSyncAllState);
        // 배틀 씬으로 둘다 넘어왔으므로 각 플레이어들을 준비해제시키고
        PhotonLogicHandler.Instance.OnUnReadyAll();
        
        // 5. 캐릭터 로드 확인
        yield return new WaitUntil(Get_CharacterSyncAllState);
        
        // 게임 시작 ㅋ
    }

    protected void GameStart()
    {
        
    }

    protected void GameStart_Failed()
    {
        CoroutineHelper.StopCoroutine(sequenceExecuteCoroutine);
        sequenceExecuteCoroutine = null;
    }

    protected bool Get_DataSyncAllState() => masterSyncData.isDataSync && slaveSyncData.isDataSync;
    protected bool Get_ReadyAllState() => masterSyncData.isReady && slaveSyncData.isReady;
    protected bool Get_SceneSyncAllState() => masterSyncData.isSceneSync && slaveSyncData.isCharacterSync;
    protected bool Get_CharacterSyncAllState() => masterSyncData.isCharacterSync && slaveSyncData.isCharacterSync;

}