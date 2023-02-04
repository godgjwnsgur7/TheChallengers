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
        if (sequenceExecuteCoroutine != null)
            CoroutineHelper.StopCoroutine(sequenceExecuteCoroutine);

        if (userSyncMediator != null)
            Managers.Resource.Destroy(userSyncMediator.gameObject);
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
        
        // 2. 동기화객체 생성 참조 확인
        yield return new WaitUntil(IsConnect_UserSyncMediator);

        // 3. 레디 확인 (마스터의 레디 == 시작 : 레디조건이 슬레이브의 준비완료가 될 것)
        yield return new WaitUntil(Get_ReadyAllState);
        Debug.Log("게임 시작");
        PhotonLogicHandler.Instance.OnGameStart(); // 게임 시작을 알림
        userSyncMediator.Sync_ShowGameInfo();

        // 4. 씬 로드 확인
        yield return new WaitUntil(Get_SceneSyncAllState);
        PhotonLogicHandler.Instance.OnUnReadyAll(); // 준비해제
        // BattleScene의 Start문에서 처리

        // 5. 캐릭터 로드 확인
        yield return new WaitUntil(Get_CharacterSyncAllState);
        userSyncMediator.Sync_GameStartEffect(); // 게임 실행
    }

    public void Start_Timer()
    {
        userSyncMediator.Start_Timer();
    }

    // Set 계열
    public void Set_UserSyncMediator(UserSyncMediator _userSyncMediator)
    {
        if (userSyncMediator != null && userSyncMediator.gameObject)
            Managers.Resource.Destroy(userSyncMediator.gameObject);

        userSyncMediator = _userSyncMediator;
    }
    public void Set_SlaveClientNickname(string _slaveClientNickname)
    {
        slaveClientNickname = _slaveClientNickname;
    }


    // Get 계열
    public bool IsConnect_UserSyncMediator() => (userSyncMediator != null);
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