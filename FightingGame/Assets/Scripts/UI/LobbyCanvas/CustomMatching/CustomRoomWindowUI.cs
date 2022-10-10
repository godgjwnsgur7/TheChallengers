using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CustomRoomWindowUI : MonoBehaviour, IRoomPostProcess
{
    public bool isInit = false;

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

    private void Init()
    {
        if (isInit) return;

        isInit = true;

        MyProfile.Set_UserNickname("닉네임 받아와야 함");
        MyProfile.Set_Character(ENUM_CHARACTER_TYPE.Default);
        MyProfile.Set_ReadyState(false);

        MyProfile.Init();
    }

    public void Open()
    {
        Init();

        // 포톤콜백함수 등록
        PhotonLogicHandler.Instance.onEnterRoomPlayer += SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += SlaveClientExitCallBack;
        PhotonLogicHandler.Instance.onChangeMasterClientNickname += MasterClientExitCallBack;

        Set_CurrRoomInfo(); // 임시로 일단 여기에 호출
        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        isInit = false;

        // 포톤콜백함수 해제
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= SlaveClientEnterCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= SlaveClientExitCallBack;
        PhotonLogicHandler.Instance.onChangeMasterClientNickname -= MasterClientExitCallBack;

        MyProfile.Clear();
        this.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 방에 최초 입장 시에 불리는 함수
    /// </summary>
    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;

        // PhotonLogicHandler.CurrentMapName의 리턴값이 Null이라 일단 주석처리
        // ENUM_MAP_TYPE mapType = (ENUM_MAP_TYPE)Enum.Parse(typeof(ENUM_CHARACTER_TYPE), PhotonLogicHandler.CurrentMapName);
        // Set_CurrMapInfo(mapType);
    }

    public void Set_CurrMapInfo(ENUM_MAP_TYPE _mapType = ENUM_MAP_TYPE.BasicMap)
    {
        switch (_mapType)
        {
            case ENUM_MAP_TYPE.BasicMap:
                // 맵 이미지 세팅해와!
                return;
            default:
                Debug.Log("알 수 없는 맵을 선택");
                return;
        }
    }

    /// <summary>
    /// 슬레이브 클라이언트 입장 시 불리는 콜백함수
    /// </summary>
    public void SlaveClientEnterCallBack(string nickname)
    {
        slaveProfile.Set_UserNickname(nickname);
    }

    /// <summary>
    /// 마스터 클라이언트가 변경됐을 때 불리는 함수 (매개변수로는 마스터 닉네임을 줌)
    /// </summary>
    public void MasterClientExitCallBack(string nickname)
    {
        MyProfile.Clear();
        MyProfile.Init(); // 닉네임 셋팅 됨
        
        if (PhotonLogicHandler.IsMasterClient) // 마스터 클라이언트가 됐을 때
        {
            MyProfile.Set_Character(slaveProfile.currCharType);

            // 슬레이브 클라이언트의 정보를 가져와서 세.
        }
        else // 슬레이브 클라이언트가 됐을 때
        {
            MyProfile.Set_Character(masterProfile.currCharType);
            slaveProfile.Set_UserNickname(nickname);
        }
    }

    /// <summary>
    /// 자신 외의 클라이언트가 나갔을 때 불리는 함수
    /// </summary>
    public void SlaveClientExitCallBack(string nickname)
    {
        slaveProfile.Clear();
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

        MyProfile.Set_Character(property.characterType);
        MyProfile.Set_ReadyState(property.isReady);
    }

    public void ExitRoom()
    {
        PhotonLogicHandler.Instance.TryLeaveRoom(Close);
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
        MyProfile.Set_ReadyState(!MyProfile.isReady);
    }

    public void OnClick_Start()
    {
        if (!PhotonLogicHandler.IsMasterClient)
            return;

        // 서로의 준비 상태를 다시 확인하고 입장시켜야 함

        // 일단 그냥 입장
        PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE.Battle);
    }
}