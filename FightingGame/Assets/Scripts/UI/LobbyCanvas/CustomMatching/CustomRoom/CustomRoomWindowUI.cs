using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CustomRoomWindowUI : UIElement, IRoomPostProcess
{
    [SerializeField] MasterProfileUI masterProfile;
    [SerializeField] SlaveProfileUI slaveProfile;

    [SerializeField] CustormRoom_MapInfo mapInfo;
    [SerializeField] UserInfoWindowUI userInfoWindow;

    [SerializeField] Text roomNameText;
    [SerializeField] Text readyOrStartText;

    [SerializeField] GameObject kickOutButtonObject;

    private bool readyLock = false;
    public bool isRoomRegisting = false;
    
    ENUM_MAP_TYPE currMap;
    public ENUM_MAP_TYPE CurrMap
    {
        get { return currMap; }
        private set
        {
            if (currMap == value)
                return;
            CurrMapInfoUpdateCallBack(value);
        }
    }

    Coroutine readyLockCoroutine;
    Coroutine waitInfoSettingCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        // 포톤콜백함수 등록
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= SlaveClientExitCallBack;
        
        PhotonLogicHandler.Instance.onEnterRoomPlayer += SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += SlaveClientExitCallBack;
        
        this.RegisterRoomCallback();

        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        readyLock = false;

        if (readyLockCoroutine != null)
            StopCoroutine(readyLockCoroutine);

        if (waitInfoSettingCoroutine != null)
            StopCoroutine(waitInfoSettingCoroutine);

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
        if(!masterProfile.IsMine) // 마스터클라이언트가 됐다면
        {
            masterProfile.Clear();
            masterProfile.Init(slaveProfile.Get_ProfileInfo());
        }

        PhotonLogicHandler.Instance.RequestUnReadyAll();
        Init();
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        if (!PhotonLogicHandler.IsMasterClient)
            CurrMap = property.currentMapType;
    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        Char rankChar;
        if (property.data.ratingPoint == 1500 && property.data.defeatPoint + property.data.victoryPoint == 0)
            rankChar = 'X';
        else
            rankChar = RankingScoreOperator.Get_RankingEmblemChar(property.data.ratingPoint);

        if (property.isMasterClient && !masterProfile.IsInit)
        {
            Profile_Info masterProfileInfo = new Profile_Info(property.data.nickname, rankChar);
            masterProfile.Init(masterProfileInfo);
        }
        else if(!property.isMasterClient && !slaveProfile.IsInit)
        {
            Profile_Info slaveProfileInfo = new Profile_Info(property.data.nickname, rankChar);
            slaveProfile.Init(slaveProfileInfo);
        }

        // 슬레이브의 정보를 마스터클라이언트가 받았을 때
        if(!property.isMasterClient && PhotonLogicHandler.IsMasterClient)
        {
            slaveProfile.Set_ReadyState(property.isReady);
        }
    }

    private void Init()
    {
        if(PhotonLogicHandler.IsMasterClient)
        {
            readyOrStartText.text = "시작";
            kickOutButtonObject.SetActive(true);
            if (!PhotonLogicHandler.IsFullRoom)
                slaveProfile.Clear();
        }
        else
        {
            readyOrStartText.text = "준비";
            kickOutButtonObject.SetActive(false);
            PhotonLogicHandler.Instance.RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
        }
    }

    public void Open()
    {
        Init();
        this.gameObject.SetActive(true);
        Set_CurrRoomInfo();

        waitInfoSettingCoroutine = StartCoroutine(IWaitInfoSetting());
    }

    public void Close()
    {
        if (!this.gameObject.activeSelf)
            return;

        Managers.Network.ExitRoom_CallBack();
        Managers.Network.Clear_DBData();

        masterProfile.Clear();
        slaveProfile.Clear();

        userInfoWindow.Close();
        this.gameObject.SetActive(false);
    }

    public void CustomRoomLeftCallBack()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Notify);
        Managers.UI.popupCanvas.DeactivePopupAll();
        Managers.UI.popupCanvas.Open_NotifyPopup("추방되었습니다.", ExitRoomCallBack);
    }

    public void CurrMapInfoUpdateCallBack(ENUM_MAP_TYPE _mapType)
    {
        if (PhotonLogicHandler.IsMasterClient)
            PhotonLogicHandler.Instance.ChangeMap(_mapType);
        else
            slaveProfile.Set_ReadyState(false);

        currMap = _mapType;
        
        mapInfo.Set_CurrMapInfo(_mapType);
    }

    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;
        CurrMap = PhotonLogicHandler.CurrentMapType;
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
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Close_CustomRoomWindow);
    }

    public void OnClick_ChangeMap(int _mapTypeNum)
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        CurrMap = (ENUM_MAP_TYPE)_mapTypeNum;
    }

    public void OnClick_KickOut()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Notify);
        Managers.UI.popupCanvas.Open_SelectPopup(KickOutToSlave, null, $"'{slaveProfile.Get_UserNickname()}' 님을 추방시키겠습니까?");
    }

    private void KickOutToSlave()
    {
        if (!PhotonLogicHandler.Instance.KickOut())
            Managers.UI.popupCanvas.Open_NotifyPopup("오류 : 강퇴에 실패했습니다.");        
    }

    public void OnClick_UserInfo(bool _isMasterProfile)
    {
        DBUserData userData = _isMasterProfile == PhotonLogicHandler.IsMasterClient ?
            Managers.Network.Get_MyDBUserData() : Managers.Network.Get_EnemyDBUserData();
        if (userData != null)
        {
            if (!Managers.UI.popupCanvas.isFadeObjActiveState)
                Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Light);

            userInfoWindow.Open(userData);
        }
    }

    public void OnClick_ExitRoom()
    {
        if(!PhotonLogicHandler.IsMasterClient)
            slaveProfile.Set_ReadyState(false);

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "방에서 나가시겠습니까?");
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_ExitRoom();
    }

    /// <summary>
    /// 마스터에겐 OnClick_Start()라고 보면 되는 상태
    /// </summary>
    public void OnClick_Ready()
    {
        if(PhotonLogicHandler.IsMasterClient)
        {
            if (!PhotonLogicHandler.IsFullRoom)
            {
                Managers.UI.popupCanvas.Open_NotifyPopup("상대방이 없습니다.");
                return;
            }
            else if(!slaveProfile.IsReady)
            {
                Managers.UI.popupCanvas.Open_NotifyPopup("상대방이 준비상태가 아닙니다.");
                return;
            }

            if (!Managers.Network.Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC))
                PhotonLogicHandler.Instance.RequestSyncDataAll();

            PhotonLogicHandler.Instance.RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.READY);
        }
        else
        {
            if (readyLock)
                return;

            if(!slaveProfile.IsReady) // 레디를 누름
            {
                Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Enter);
                readyOrStartText.text = "준비 해제";
            }
            else // 레디를 취소
            {
                Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Cilck_Heavy1);
                readyOrStartText.text = "준비";
            }
            
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

    protected IEnumerator IWaitInfoSetting()
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            yield return new WaitUntil(() => masterProfile.IsInit);

            // 커스텀 매치를 마치고 돌아온 경우
            if (PhotonLogicHandler.IsFullRoom)
            {
                Managers.Network.Start_SequenceExecuter();

                yield return new WaitUntil(() => slaveProfile.IsInit || PhotonLogicHandler.IsFullRoom == false);
            }
        }
        else
        {
            yield return new WaitUntil(() => masterProfile.IsInit && slaveProfile.IsInit);
        }

        Managers.UI.popupCanvas.Play_FadeInEffect();
    }
}