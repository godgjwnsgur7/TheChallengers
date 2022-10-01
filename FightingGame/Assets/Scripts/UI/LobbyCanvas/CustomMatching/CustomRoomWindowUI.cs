using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CustomRoomWindowUI : MonoBehaviourPhoton
{
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

    public bool isInit = false;

    public override void Init()
    {
        if (isInit) return;

        base.Init();

        isInit = true;
        myProfile.isMine = true;
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

    


    #region OnClick 함수들
    public void OnClick_ChangeMap()
    {
        Debug.Log("맵 변경시켜. (미구현)");
        // 맵 변경시켜.
    }

    public void OnClick_ExitRoom()
    {
        if(!PhotonLogicHandler.IsMasterClient)
        {
            // 준비해제시켜.
        }

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "정말 방에서 나가시겠습니까?");
    }

    public void OnClick_ReadyOrStart()
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            // 마스터 클라이언트

        }
        else
        {
            // 슬레이브 클라이언트

        }
    }
    #endregion
}
