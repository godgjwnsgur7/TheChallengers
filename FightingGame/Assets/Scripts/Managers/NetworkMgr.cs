using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 게임의 전반적인 관리를 할 매니저
/// 서버접속상태 체크, 네트워크 끊김 등을 판단
/// </summary>
public class NetworkMgr : IRoomPostProcess
{
    UserSyncMediator userSyncMediator = null;

    DBUserData masterDBData;
    DBUserData slaveDBData;

    Coroutine sequenceExecuteCoroutine = null;

    string slaveClientNickname = null;

    public bool IsServerSyncState
    {
        get
        {
            return (userSyncMediator != null);
        }
    }

    ENUM_CHARACTER_TYPE myCharType = ENUM_CHARACTER_TYPE.Default;

    public ENUM_CHARACTER_TYPE Get_MyCharType() => myCharType;

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
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= OnEnterRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnExitRoomCallBack;
    }

    public void OnEnterRoomCallBack(string enterUserNickname)
    {
        // 임시 주석처리
        //if (PhotonLogicHandler.CurrentMyNickname == enterUserNickname)
        //    return;

        if (PhotonLogicHandler.IsMasterClient)
        {
            slaveClientNickname = enterUserNickname;
            Start_SequenceExecuter();
            PhotonLogicHandler.Instance.RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
        }
    }

    public void OnExitRoomCallBack(string exitUserNickname)
    {
        slaveDBData = null;
        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();

        ExitRoom_CallBack();
    }

    public void ExitRoom_CallBack()
    {
        if (sequenceExecuteCoroutine != null)
        {
            CoroutineHelper.StopCoroutine(sequenceExecuteCoroutine);
            sequenceExecuteCoroutine = null;
        }

        if (userSyncMediator != null)
        {
            Managers.Resource.Destroy(userSyncMediator.gameObject);
            userSyncMediator = null;
        }
	}

    public void OnUpdateRoomProperty(CustomRoomProperty property) { }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (property.isMasterClient)
            masterDBData = property.data;
        else
            slaveDBData = property.data;

        if (property.isMasterClient == PhotonLogicHandler.IsMasterClient)
            myCharType = property.characterType;
    }

    public void Clear_DBData()
    {
        masterDBData = null;
        slaveDBData = null;
    }

    public void Register_TimerCallBack(Action<int> _updateTimerCallBack)
    {
        userSyncMediator.Register_TimerCallBack(_updateTimerCallBack);
    }

    public void Start_SequenceExecuter()
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        if(sequenceExecuteCoroutine != null)
            CoroutineHelper.StopCoroutine(sequenceExecuteCoroutine);

        sequenceExecuteCoroutine = CoroutineHelper.StartCoroutine(INetworkSequenceExecuter());
    }

    protected IEnumerator INetworkSequenceExecuter()
    {
        if (!PhotonLogicHandler.IsMasterClient || !PhotonLogicHandler.IsFullRoom)
        {
            Debug.Log($"Stop SequenceExecuter" +
                $"\nIsMasterClient : {PhotonLogicHandler.IsMasterClient}" +
                $"\nIsFullRoom : {PhotonLogicHandler.IsFullRoom}");
            yield break;
        }

        // 0. 데이터 싱크
        PhotonLogicHandler.Instance.RequestSyncDataAll();

        // 1. 연결 확인
        yield return new WaitUntil(() => Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC));

        if (PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.RANDOM)
        {
            yield return new WaitForSeconds(0.5f);
            PhotonLogicHandler.Instance.RequestReadyAll();
        }

        // 2. 레디 확인 (마스터의 레디 == 시작 : 레디조건이 슬레이브의 준비완료가 될 것)
        yield return new WaitUntil(() => Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.READY));
        PhotonLogicHandler.Instance.RequestGameStart(); // 게임 시작 상태로 변경

        // 게임 돌입할 때 UserSyncMediator 생성
        if (userSyncMediator != null)
            Managers.Resource.Destroy(userSyncMediator.gameObject);
        if (PhotonLogicHandler.IsJoinedRoom && PhotonLogicHandler.IsFullRoom)
            Managers.Resource.InstantiateEveryone("UserSyncMediator");
        else
            yield break;

        // 3. 동기화객체 생성 참조 확인
        yield return new WaitUntil(IsConnect_UserSyncMediator);
        userSyncMediator.Sync_ShowGameInfo(); // 캐릭터 선택 창 돌입

        // 4. 캐릭터 선택 확인
        yield return new WaitUntil(() => Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER));
        userSyncMediator.Sync_ShowGameStartInfo(); // 양 쪽의 정보를 보여주고 배틀씬 이동

        // 5. 씬 로드 확인
        yield return new WaitUntil(() => Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC));
        // 캐릭터 소환은 이 시점에 BattleScene의 Start문에서 처리됨

        // 6. 캐릭터 로드 확인
        yield return new WaitUntil(() => Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER_SYNC));
        userSyncMediator.Sync_GameStartEffect(); // 게임 실행

        // 7. 커스텀 플레이어 프로퍼티 데이터 값 초기화
        userSyncMediator.Sync_RequestUnSyncDataAll();

        // 이후에 강제종료, 등 예외처리들이 필요할 것으로 보임

        sequenceExecuteCoroutine = null;
    }

    /// <summary>
    /// 마스터 서버 연결 후에 연결이 끊길 경우 호출
    /// </summary>
    public void DisconnectMaster_CallBack(string cause)
    {
        Time.timeScale = 0;
        Managers.UI.popupCanvas.Open_NotifyPopup($"{cause} !" +
            $"\n서버와 연결이 끊어졌습니다.", GoTo_Main);
    }

    public void GoTo_Main()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Main);
    }

    public void EndGame_GoToLobby()
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            PhotonLogicHandler.Instance.RequestGameEnd();
        }

        Managers.UI.popupCanvas.Play_FadeOutEffect(DestroyAll_PhotonObject);
    }

    /// <summary>
    /// 양쪽 클라이언트의 포톤 객체가 사라질 때까지 대기했다가
    /// 삭제가 완료되면 로비씬으로 이동
    /// </summary>
    protected IEnumerator IWaitDestroyAllPhotonObject()
    {
        yield return PhotonLogicHandler.Instance.TryDestroyAllPhotonOnScene(null);

        if(PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.RANDOM)
        {
			PhotonLogicHandler.Instance.TryLeaveRoom(() =>
			{
				Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
			});
		}
        else if(PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.CUSTOM)
        {
			if (PhotonLogicHandler.IsMasterClient)
            {
				Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Lobby);
			}	
		}
	}

    public void DestroyAll_PhotonObject()
    {
        CoroutineHelper.StartCoroutine(IWaitDestroyAllPhotonObject());
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

    public bool Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES _properties)
    {
        return PhotonLogicHandler.Instance.CheckAllPlayerProperty(_properties);
    }

    public string Get_SlaveClientNickname() => slaveClientNickname;

    public DBUserData Get_DBUserData(bool _isMasterClient)
    {
        if (_isMasterClient)
            return masterDBData;
        else
            return slaveDBData;
    }
}