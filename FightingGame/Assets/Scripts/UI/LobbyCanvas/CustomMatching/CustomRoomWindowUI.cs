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
            {
                readyOrStartText.text = "시작";
                return masterProfile;
            }
            else
            {
                readyOrStartText.text = "준비";
                return slaveProfile;
            }
        }
    }
    [SerializeField] Text roomNameText;
    [SerializeField] Text readyOrStartText;

    [SerializeField] Button[] mapImageButtons = new Button[3];
    [SerializeField] Image currMapImage;
    [SerializeField] Image selectionEffectImage;
    [SerializeField] Text mapNameText;
    [SerializeField] Text mapExplanationText;

    private bool isInit = false;

    private bool readyLock = false;
    public bool isRoomRegisting = false;
    
    ENUM_MAP_TYPE currMap;
    public ENUM_MAP_TYPE CurrMap
    {
        get { return currMap; }
        private set { CurrMapInfoUpdateCallBack(value); }
    }

    Coroutine readyLockCoroutine;

    private void OnEnable()
    {
        // 포톤콜백함수 등록
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= SlaveClientExitCallBack;
        
        PhotonLogicHandler.Instance.onEnterRoomPlayer += SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += SlaveClientExitCallBack;
        
        this.RegisterRoomCallback();

        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    private void OnDisable()
    {
        if(readyLockCoroutine != null)
            StopCoroutine(readyLockCoroutine);

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
            Managers.Network.Set_SlaveClientNickname(nickname);
    }

    /// <summary>
    /// 자신 외의 다른 클라이언트가 방을 나가면 불리는 콜백함수
    /// ( 이 경우, 자신이 무조건 마스터 클라이언트가 됨 )
    /// </summary>
    public void SlaveClientExitCallBack(string nickname)
    {
        if(!masterProfile.isMine) // 마스터클라이언트가 됐다면
        {
            PhotonLogicHandler.Instance.OnUnReady();
            masterProfile.Clear();
            masterProfile.Init();
            masterProfile.Set_UserNickname(PhotonLogicHandler.CurrentMyNickname);
            masterProfile.Set_RankingEmblem(slaveProfile.Get_RankingEmblem());
        }

        slaveProfile.Clear();
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        if (PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보면 리턴

        if (property.isStarted) // 게임 시작을 알림받음
        {
            Debug.Log("실행확인");
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
            masterProfile.Set_RankingEmblem(property.data.ratingPoint);
        }
        else
        {
            slaveProfile.Set_ReadyState(property.isReady);
            slaveProfile.Set_RankingEmblem(property.data.ratingPoint);
        }
    }

    private void Init()
    {
        MyProfile.Init();
    }

    public void Open()
    {
        Init();
        Set_CurrRoomInfo();
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        if (!this.gameObject.activeSelf)
            return;

        isInit = false;

        masterProfile.Clear();
        slaveProfile.Clear();

        this.gameObject.SetActive(false);
    }

    public void CurrMapInfoUpdateCallBack(ENUM_MAP_TYPE _mapType)
    {
        if (PhotonLogicHandler.IsMasterClient)
            PhotonLogicHandler.Instance.ChangeMap(_mapType);

        currMap = _mapType;
        currMapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);
        mapExplanationText.text = Managers.Data.Get_MapExplanationDict(_mapType);

        RectTransform selectionEffectImageRectTr = selectionEffectImage.gameObject.GetComponent<RectTransform>();
        selectionEffectImageRectTr.anchoredPosition = mapImageButtons[(int)_mapType].GetComponent<RectTransform>().anchoredPosition;
    }

    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;
        MyProfile.Set_UserNickname(PhotonLogicHandler.CurrentMyNickname);

        if (!PhotonLogicHandler.IsMasterClient)
            masterProfile.Set_UserNickname(PhotonLogicHandler.CurrentMasterClientNickname);
        else if (PhotonLogicHandler.IsFullRoom && slaveProfile.Get_UserNickname() == "")
            slaveProfile.Set_UserNickname(Managers.Network.Get_SlaveClientNickname());  
        
        CurrMap = PhotonLogicHandler.CurrentMapType;
        PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
    }

    public void ExitRoom()
    {
        bool isLeaveRoom = PhotonLogicHandler.Instance.TryLeaveRoom(ExitRoomCallBack);

        if(!isLeaveRoom)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("알 수 없는 에러\n나가기 실패");
        }
    }

    private void ExitRoomCallBack()
    {
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Close_CustomRoomWindow();
    }

    public void OnClick_ChangeMap(int _mapTypeNum)
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        CurrMap = (ENUM_MAP_TYPE)_mapTypeNum;
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
            if (!PhotonLogicHandler.IsFullRoom || !slaveProfile.IsReady)
            {
                Managers.UI.popupCanvas.Open_NotifyPopup("모든 유저가 준비상태가 아닙니다.");
                return;
            }

            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.READY);
        }
        else
        {
            if (readyLock)
                return;

            slaveProfile.Set_ReadyState(!slaveProfile.IsReady);
            readyLockCoroutine = StartCoroutine(IReadyButtonLock(2.0f));
        }
    }
    protected IEnumerator IReadyButtonLock(float waitTime)
    {
        readyLock = true;
        yield return new WaitForSeconds(waitTime);
        readyLock = false;
        readyLockCoroutine = null;
    }
}