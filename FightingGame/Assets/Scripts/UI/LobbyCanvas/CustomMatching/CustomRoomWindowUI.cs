using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CustomRoomWindowUI : MonoBehaviour
{
    public bool isInit = false;

    [SerializeField] Text roomNameText;

    [SerializeField] Image currMapIamge;
    [SerializeField] Image nextMapIamge_Left;
    [SerializeField] Image nextMapIamge_Right;

    [SerializeField] CharProfileUI masterProfile;
    [SerializeField] CharProfileUI slaveProfile;

    CharProfileUI myProfile
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
        myProfile.isMine = true;

        myProfile.Set_UserNickname("닉네임 받아와야 함");
        myProfile.Set_Character(ENUM_CHARACTER_TYPE.Default);
        myProfile.Set_ReadyState(false);
        
        myProfile.Init();
    }

    public void Open()
    {
        Init();

        Set_CurrRoomInfo(); // 임시로 일단 여기에 호출

        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        isInit = false;

        myProfile.Clear();
        this.gameObject.SetActive(false);
    }

    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;

        // PhotonLogicHandler.CurrentMapName 이 Null로 반환되서 일단 주석처리
        // Set_CurrMapInfo((ENUM_MAP_TYPE)Enum.Parse(typeof(ENUM_CHARACTER_TYPE), PhotonLogicHandler.CurrentMapName));
    }

    public void Set_CurrMapInfo(ENUM_MAP_TYPE _mapType)
    {
        switch(_mapType)
        {
            case ENUM_MAP_TYPE.BasicMap:
                // 맵 이미지 세팅해와!
                return;
            default:
                Debug.Log("알 수 없는 맵을 선택함 이게 가능함?ㅋㅋ");
                return;
        }
    }

    public void ExitRoom()
    {
        bool isExit = PhotonLogicHandler.Instance.TryLeaveRoom(Close);
        if (!isExit)
            Managers.UI.popupCanvas.Open_NotifyPopup("방에서 나가지 못했습니다.");
    }

    public void OnClick_ChangeMap()
    {
        Debug.Log("맵 변경시켜. (미구현)");
        // 맵 변경시켜.
    }

    public void OnClick_ExitRoom()
    {
        myProfile.Set_ReadyState(false);

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "정말 방에서 나가시겠습니까?");
    }

    public void OnClick_Ready()
    {
        myProfile.Set_ReadyState(!myProfile.isReady);
    }

    public void GoTo_BattleScene()
    {
        // 같이 배틀 씬 입장

    }
}
