using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CustomRoomWindowUI : MonoBehaviour, IRoomPostProcess
{
    [SerializeField] Text roomNameText;

    [SerializeField] Image currMapIamge;
    [SerializeField] Image nextMapIamge_Left;
    [SerializeField] Image nextMapIamge_Right;

    [SerializeField] CharProfileUI masterProfile;
    [SerializeField] CharProfileUI slaveProfile;
    CharProfileUI MyProfile
    {
        get
        {
            if (PhotonLogicHandler.IsMasterClient)
                return masterProfile;
            else
                return slaveProfile;
        }
    }
    CharProfileUI YourProfile
    {
        get
        {
            if (PhotonLogicHandler.IsMasterClient)
                return slaveProfile;
            else
                return masterProfile;
        }
    }

    public bool isInit = false;
    public bool isRoomRegisting = false;
    bool isReadyLock = false;

    Coroutine readyLockCoroutine;
    Coroutine allReadyCheckCoroutine;

    private void Init()
    {
        if (isInit) return;

        isInit = true;

        MyProfile.Init();
    }

    public void Open()
    {
        Register_LobbyCallback();

        Init();

        // 포톤콜백함수 등록
        PhotonLogicHandler.Instance.onEnterRoomPlayer += SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += SlaveClientExitCallBack;
        PhotonLogicHandler.Instance.onChangeMasterClientNickname += MasterClientExitCallBack;

        Set_CurrRoomInfo();
        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        isInit = false;

        // 포톤콜백함수 해제
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= SlaveClientExitCallBack;
        PhotonLogicHandler.Instance.onChangeMasterClientNickname -= MasterClientExitCallBack;

        masterProfile.Clear();
        slaveProfile.Clear();

        UnRegister_LobbyCallback();
        this.gameObject.SetActive(false);
    }

    #region Register
    public bool Register_LobbyCallback()
    {
        if (isRoomRegisting)
        {
            Debug.Log("이미 등록된 상태");
            return false;
        }

        isRoomRegisting = true;
        this.RegisterRoomCallback();
        return true;
    }
    public void UnRegister_LobbyCallback()
    {
        if (!isRoomRegisting)
            return;

        isRoomRegisting = false;
        this.UnregisterRoomCallback();
    }
    #endregion

    public void ExitRoom()
    {
        PhotonLogicHandler.Instance.TryLeaveRoom(Close);
    }

    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;
        MyProfile.Set_UserNickname(PhotonLogicHandler.CurrentMyNickname);
        
        if (!PhotonLogicHandler.IsMasterClient)
            YourProfile.Set_UserNickname(PhotonLogicHandler.CurrentMasterClientNickname);
        

        // Error : ArgumentNullException: Value cannot be null.
        // ENUM_MAP_TYPE mapType = (ENUM_MAP_TYPE)Enum.Parse(typeof(ENUM_CHARACTER_TYPE), PhotonLogicHandler.CurrentMapName);
        // Set_CurrMapInfo(mapType);
    }

    public void Set_CurrMapInfo(ENUM_MAP_TYPE _mapType = ENUM_MAP_TYPE.BasicMap)
    {
        switch (_mapType)
        {
            case ENUM_MAP_TYPE.BasicMap:
                PhotonLogicHandler.Instance.ChangeMap(_mapType);
                return;
            default:
                Debug.Log("알 수 없는 맵을 선택");
                return;
        }
    }

    public void SlaveClientEnterCallBack(string nickname)
    {
        if (PhotonLogicHandler.IsMasterClient)
            YourProfile.Set_UserNickname(nickname);
    }


    public void MasterClientExitCallBack(string nickname)
    {
        MyProfile.Clear();
        MyProfile.Init();
        
        if (PhotonLogicHandler.IsMasterClient)
        {
            MyProfile.Set_Character(slaveProfile.currCharType);

            // 슬레이브 클라이언트의 정보를 가져와서 세팅해야 함
        }
        else
        {
            // 아직 이럴 일이 없다~ 나중에 가자
        }
    }

    public void SlaveClientExitCallBack(string nickname)
    {
        YourProfile.Clear();
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        if (PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보면 리턴

        Set_CurrMapInfo(property.currentMapType);
    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (property.isMasterClient == PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보면 리턴

        Debug.Log("상대에게 정보를 받아 갱신합니다.");
        YourProfile.Set_Character(property.characterType);
        YourProfile.Set_ReadyState(property.isReady);
    }

    public void OnClick_ChangeMap()
    {
        Debug.Log("맵 변경시켜. (미구현)");
        // 맵 변경시켜.
    }

    public void OnClick_ExitRoom()
    {
        MyProfile.Set_ReadyState(false);

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "정말 방에서 나가시겠습니까?");
    }

    public void OnClick_Ready()
    {
        if (isReadyLock)
            return;

        readyLockCoroutine = StartCoroutine(IReadyButtonLock(2f));

        MyProfile.Set_ReadyState(!MyProfile.IsReady);

    }

    public void OnClick_Start()
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        // 서로의 준비 상태를 다시 확인하고 입장시켜야 함

        // 일단 그냥 입장
        PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE.Battle);
    }



    protected IEnumerator IReadyButtonLock(float waitTime)
    {
        isReadyLock = true;

        yield return new WaitForSeconds(waitTime);

        isReadyLock = false;
    }

    protected IEnumerator IAllReadyCheck()
    {
        bool allReadyState;
        int CurrRoomMemberCount;

        while (MyProfile.IsReady)
        {
            allReadyState = PhotonLogicHandler.Instance.IsAllReady();
            CurrRoomMemberCount = PhotonLogicHandler.CurrentRoomMemberCount;

            if (allReadyState && CurrRoomMemberCount == 2)
            {
                Debug.Log("둘 다 준비가 완료됨");
                // PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE.Battle);

                break;
            }
            yield return null;
        }

        allReadyCheckCoroutine = null;
    }

}