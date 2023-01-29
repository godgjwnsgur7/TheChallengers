using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// Ready, DataSync, SceneSync, CharacterSync, characterType
/// </summary>
public class SyncData
{
    public bool isDataSync;
    public bool isReady;
    public bool isSceneSync;
    public bool isCharacterSync;
    public ENUM_CHARACTER_TYPE characterType;

    public SyncData(bool _isDataSync, bool _isReady, bool _isSceneSync, bool _isCharacterSync, ENUM_CHARACTER_TYPE _characterType)
    {
        isDataSync = _isDataSync;
        isReady = _isReady;
        isSceneSync = _isSceneSync;
        isCharacterSync = _isCharacterSync;
        characterType = _characterType;
    }
}

/// <summary>
/// 게임의 전반적인 관리를 할 매니저
/// 서버접속상태 체크, 네트워크 끊김 등을 판단
/// </summary>
public class NetworkMgr : IRoomPostProcess
{
    UserSyncMediator userSyncMediator = null;

    SyncData masterSyncData = new SyncData(false, false, false, false, ENUM_CHARACTER_TYPE.Default);
    SyncData slaveSyncData = new SyncData(false, false, false, false, ENUM_CHARACTER_TYPE.Default);

    DBUserData myDBData;
    DBUserData enemyDBData;

    Coroutine sequenceExecuteCoroutine = null;

    string slaveClientNickname = null;

    public bool isServerSyncState
    {
        get
        {
            return (Get_DataSyncAllState() && userSyncMediator != null);
        }
    }

    public void Init()
    {
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= OnEnterRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnExitRoomCallBack;

        PhotonLogicHandler.Instance.onEnterRoomPlayer += OnEnterRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += OnExitRoomCallBack;

        this.RegisterRoomCallback();
    }

    public void Clear()
    {

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
            return;

        if (PhotonLogicHandler.IsMasterClient)
        {
            slaveClientNickname = enterUserNickname;
            sequenceExecuteCoroutine = CoroutineHelper.StartCoroutine(INetworkSequenceExecuter());
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
        }
    }

    public void OnExitRoomCallBack(string exitUserNickname)
    {
        if (userSyncMediator != null)
            Managers.Resource.Destroy(userSyncMediator.gameObject);

        // 나간 유저가 내가 아니라면 (내가 원래 마스터클라이언트였다면)
        if (PhotonLogicHandler.CurrentMyNickname != exitUserNickname)
        {
            if (sequenceExecuteCoroutine != null)
                CoroutineHelper.StopCoroutine(sequenceExecuteCoroutine);
        }
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {

    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        SyncData _syncData = new SyncData(property.isDataSync, property.isReady, property.isSceneSync, property.isCharacterSync, property.characterType);

        if (property.isMasterClient)
        {
            masterSyncData = _syncData;
            myDBData = property.data;
        }
        else
        {
            slaveSyncData = _syncData;
            enemyDBData = property.data;
        }
    }

    public void Register_TimerCallBack(Action<int> _updateTimerCallBack)
    {
        userSyncMediator.Register_TimerCallBack(_updateTimerCallBack);
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
        Managers.Resource.InstantiateEveryone("UserSyncMediator"); // 유저싱크메디에이터 생성
        Debug.Log("동기화 완료");

        // 2. 레디 확인 (마스터의 레디 == 시작 : 레디조건이 슬레이브의 준비완료가 될 것)
        yield return new WaitUntil(Get_ReadyAllState);
        Debug.Log("게임 시작");
        PhotonLogicHandler.Instance.OnGameStart(); // 게임 시작을 알림
        PhotonLogicHandler.Instance.OnUnReadyAll(); // 둘다 준비해제 시킴
        userSyncMediator.Sync_ShowGameInfo();

        // 4. 씬 로드 확인
        yield return new WaitUntil(Get_SceneSyncAllState);
        // 배틀 씬으로 둘다 넘어왔으므로 각 플레이어들을 준비해제시키고
        PhotonLogicHandler.Instance.OnUnReadyAll();

        // 양측에 캐릭터를 생성


        // 5. 캐릭터 로드 확인
        yield return new WaitUntil(Get_CharacterSyncAllState);

        // 게임 시작 ㅋ
    }

    protected void GameStart()
    {

    }

    // Get 계열
    protected bool Get_DataSyncAllState()
    {
        Debug.Log($"masterSyncData.isDataSync : {masterSyncData.isDataSync}");
        Debug.Log($"slaveSyncData.isDataSync : {slaveSyncData.isDataSync}");

        return masterSyncData.isDataSync && slaveSyncData.isDataSync;
    }
    protected bool Get_ReadyAllState()
    {
        Debug.Log($"masterSyncData.isReady : {masterSyncData.isReady}");
        Debug.Log($"slaveSyncData.isReady : {slaveSyncData.isReady}");

        return masterSyncData.isReady && slaveSyncData.isReady;
    }
    public bool Get_SceneSyncAllState()
    {
        Debug.Log($"masterSyncData.isSceneSync : {masterSyncData.isSceneSync}");
        Debug.Log($"slaveSyncData.isSceneSync : {slaveSyncData.isSceneSync}");

        return masterSyncData.isSceneSync && slaveSyncData.isSceneSync;
    }

    protected bool Get_CharacterSyncAllState()
    {
        Debug.Log($"masterSyncData.isCharacterSync : {masterSyncData.isCharacterSync}");
        Debug.Log($"slaveSyncData.isCharacterSync : {slaveSyncData.isCharacterSync}");

        return masterSyncData.isCharacterSync && slaveSyncData.isCharacterSync;
    }

    public bool Get_SyncState() => userSyncMediator != null;
    public string Get_SlaveClientNickname() => slaveClientNickname;
    public ENUM_CHARACTER_TYPE Get_MyCharacterType()
    {
        if (PhotonLogicHandler.IsMasterClient)
            return masterSyncData.characterType;
        else
            return slaveSyncData.characterType;    
    }
    public ENUM_CHARACTER_TYPE Get_EmenyCharType()
    {
        if (PhotonLogicHandler.IsMasterClient)
            return slaveSyncData.characterType;
        else
            return masterSyncData.characterType;
    }
    public DBUserData Get_DBUserData(bool _isMasterClient)
    {
        if (_isMasterClient)
            return myDBData;
        else
            return enemyDBData;
    }
}