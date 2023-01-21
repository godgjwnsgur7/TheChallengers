using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ready, DataSync, SceneSync, CharacterSync
/// </summary>
public class SyncData
{
    public bool isDataSync;
    public bool isReady;
    public bool isSceneSync;
    public bool isCharacterSync;

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
/// 
/// 얘는 순차적으로 실행할 것
/// yield return new WaitUntil 활용
/// 실행시점 제어해야 해
/// 
/// </summary>
public class NetworkMgr : IRoomPostProcess
{
    UserSyncMediator userSyncMediator = null;

    SyncData masterSyncData = null;
    SyncData slaveSyncData = null;

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

        }
        else // 상대가 입장
        {
            if(PhotonLogicHandler.IsMasterClient)
            {
                Managers.Resource.InstantiateEveryone("UserSyncMediator");
                
                
            }
        }
    }

    public void OnExitRoomCallBack(string exitUserNickname)
    {
        if (PhotonLogicHandler.CurrentMyNickname != exitUserNickname)
        {
            Managers.Resource.Destroy(userSyncMediator.gameObject);
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

    public void Clear()
    {
        this.UnregisterRoomCallback();
    }

    protected IEnumerator INetworkSequenceExecuter()
    {
        if (!PhotonLogicHandler.IsMasterClient)
            yield break;

        // 1. 연결 확인
        yield return new WaitUntil(Get_SceneSyncAllState);

        // 2. 레디 확인
        yield return new WaitUntil(Get_ReadyAllState);

        // 3. 마스터한테 게임 시작할거냐고 물어봐
        

        // 4. 시작했다면, 양쪽 레디가 됐는지 다시 확인해
        if (!Get_ReadyAllState())
        {
            GameStart_Failed();
            yield break;
        }

        // 여기에 씬 로드하기 전에 필요한 처리과정


        // 5. 게임에 시작해 (씬이동하시고, 로드가 됐는지는 씬에서 판단해서 받아)
        yield return new WaitUntil(Get_SceneSyncAllState);        

        // 6. 씬 로드 확인
        yield return new WaitUntil(Get_SceneSyncAllState);

        // 7. 캐릭터 로드 확인

    }

    protected void GameStart_Failed()
    {
        
    }

    protected bool Get_DataSyncAllState() => masterSyncData.isDataSync && slaveSyncData.isDataSync;
    protected bool Get_ReadyAllState() => masterSyncData.isReady && slaveSyncData.isReady;
    protected bool Get_SceneSyncAllState() => masterSyncData.isSceneSync && slaveSyncData.isCharacterSync;
    protected bool Get_CharacterSyncAllState() => masterSyncData.isCharacterSync && slaveSyncData.isCharacterSync;

    
}