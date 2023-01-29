using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CustomRoomWindowUI : MonoBehaviour, IRoomPostProcess
{
    [SerializeField] MasterProfileUI masterProfile;
    [SerializeField] SlaveProfileUI slaveProfile;
    BaseProfile MyProfile
    {
        get
        {
            if (PhotonLogicHandler.IsMasterClient)
                return masterProfile;
            else
                return slaveProfile;
        }
    }

    [SerializeField] Image currMapIamge;
    [SerializeField] Image nextMapIamge_Left;
    [SerializeField] Image nextMapIamge_Right;

    [SerializeField] Text roomNameText;
    [SerializeField] Text mapNameText;

    private bool isInit = false;
    public bool isRoomRegisting = false;
    
    ENUM_MAP_TYPE currMap;
    public ENUM_MAP_TYPE CurrMap
    {
        get { return currMap; }
        private set { CurrmapInfoUpdateCallBack(value); }
    }

    private void OnEnable()
    {
        // 포톤콜백함수 등록
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= SlaveClientExitCallBack;
        
        PhotonLogicHandler.Instance.onEnterRoomPlayer += SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += SlaveClientExitCallBack;
        
        this.RegisterRoomCallback();
    }

    private void OnDisable()
    {
        // 포톤콜백함수 해제
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= SlaveClientExitCallBack;

        this.UnregisterRoomCallback();
    }

    /// <summary>
    /// 슬레이브 클라이언트가 방에 입장하면 불리는 콜백함수
    /// </summary>
    public void SlaveClientEnterCallBack(string nickname)
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            slaveProfile.Set_UserNickname(nickname);
            Managers.Battle.Set_SlaveNickname(nickname);
        }

        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    /// <summary>
    /// 자신 외의 다른 클라이언트가 방을 나가면 불리는 콜백함수
    /// ( 이 경우, 자신이 무조건 마스터 클라이언트가 됨 )
    /// </summary>
    public void SlaveClientExitCallBack(string nickname)
    {
        slaveProfile.Clear();

        if(!masterProfile.isMine) // 마스터클라이언트가 됐다면
        {
            PhotonLogicHandler.Instance.OnUnReady();
            masterProfile.Clear();
            masterProfile.Init();
            masterProfile.Set_UserNickname(PhotonLogicHandler.CurrentMyNickname);
        }
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        if (PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보이거나 시작중이라면 리턴

        if (property.isStarted) // 게임 시작을 알림받음
        {
            Managers.UI.popupCanvas.Play_FadeOutEffect(Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_FightingInfoWindow);
            return;
        }

        CurrMap = property.currentMapType;
    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (property.isMasterClient == PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보면 리턴
        
        if(property.isMasterClient)
        {
            masterProfile.Set_Character(property.characterType);
        }
        else
        {
            slaveProfile.Set_Character(property.characterType);
            slaveProfile.Set_ReadyState(property.isReady);
        }
    }

    private void Init()
    {
        if (isInit) return;

        isInit = true;

        MyProfile.Init();
    }

    public void Open()
    {
        if (this.gameObject.activeSelf)
        {
            Debug.Log("커스텀 룸 윈도우가 이미 열려있습니다.");
        }
        else
        {
            this.gameObject.SetActive(true);
            Init();
        }

        Set_CurrRoomInfo();
    }

    public void Close()
    {
        if (!this.gameObject.activeSelf)
            return;

        isInit = false;

        masterProfile.Clear();
        slaveProfile.Clear();

        this.gameObject.SetActive(false);
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Close_CustomMatchingWindow();
    }

    public void CurrmapInfoUpdateCallBack(ENUM_MAP_TYPE _mapType)
    {
        if (PhotonLogicHandler.IsMasterClient)
            PhotonLogicHandler.Instance.ChangeMap(_mapType);

        currMap = _mapType;
        currMapIamge.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);

        int mapIndex = (int)_mapType - 1;
        if (mapIndex <= 0)
            mapIndex = (int)ENUM_MAP_TYPE.Max - 1;
        nextMapIamge_Left.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{(ENUM_MAP_TYPE)mapIndex}");

        mapIndex = (int)_mapType + 1;
        if (mapIndex >= (int)ENUM_MAP_TYPE.Max)
            mapIndex = 0;
        nextMapIamge_Right.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{(ENUM_MAP_TYPE)mapIndex}");
    }

    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;
        MyProfile.Set_UserNickname(PhotonLogicHandler.CurrentMyNickname);

        Debug.Log("1");

        if (!PhotonLogicHandler.IsMasterClient)
        {

            Debug.Log("2");
            masterProfile.Set_UserNickname(PhotonLogicHandler.CurrentMasterClientNickname);
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
        }
        else if (PhotonLogicHandler.IsFullRoom && slaveProfile.Get_UserNickname() == "")
            slaveProfile.Set_UserNickname(Managers.Battle.Get_SlaveNickname());  
            
        CurrMap = PhotonLogicHandler.CurrentMapType;
    }

    // 얘를 다른 곳으로 옮겨줘야 하고
    public void GoTo_BattleScene()
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        if (PhotonLogicHandler.Instance.IsAllReady() && PhotonLogicHandler.IsFullRoom)
        {
            PhotonLogicHandler.Instance.OnUnReadyAll(); // 모두 준비해제 시키고
            PhotonLogicHandler.Instance.OnGameStart(); // 게임 시작을 알림

            Managers.UI.popupCanvas.Play_FadeOutEffect(Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_FightingInfoWindow);
        }
        else
            Managers.UI.popupCanvas.Open_NotifyPopup("게임 시작에 실패했습니다.");
    }

    public void ExitRoom()
    {
        bool isLeaveRoom = PhotonLogicHandler.Instance.TryLeaveRoom(Close);

        if(!isLeaveRoom)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("알 수 없는 에러\n나가기 실패");
        }
    }

    public void OnClick_ChangeMap(bool _isRight)
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        int _mapIndex = (int)CurrMap;

        if (_isRight)
        {
            _mapIndex += 1;
            if (_mapIndex >= (int)ENUM_MAP_TYPE.Max)
                _mapIndex = 0;
        }
        else
        {
            _mapIndex -= 1;
            if (_mapIndex <= 0)
                _mapIndex = (int)ENUM_MAP_TYPE.Max - 1;
        }

        CurrMap = (ENUM_MAP_TYPE)_mapIndex;
    }

    public void OnClick_ExitRoom()
    {
        if(!PhotonLogicHandler.IsMasterClient)
            slaveProfile.Set_ReadyState(false);

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "정말 방에서 나가시겠습니까?");
    }

    /// <summary>
    /// 마스터에겐 OnClick_Start()라고 보면 되는 상태
    /// </summary>
    public void OnClick_Ready()
    {
        if(PhotonLogicHandler.IsMasterClient)
        {
            if(PhotonLogicHandler.IsFullRoom && slaveProfile.IsReady)
            {
                PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.READY);
            }
            else
            {
                Managers.UI.popupCanvas.Open_NotifyPopup("모든 유저가 준비상태가 아닙니다.");
            }
        }
        else
        {
            slaveProfile.Set_ReadyState(!slaveProfile.IsReady);
        }
    }
}