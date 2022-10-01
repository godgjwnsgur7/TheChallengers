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

    [SerializeField] GameObject readyButtonObject;
    [SerializeField] GameObject startButtonObject;

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

        if (PhotonLogicHandler.IsMasterClient)
            MasterClientSetting();
        else
            SlaveClientSetting();
    }

    public void Open()
    {
        Init();

        Set_CurrRoomInfo(); // 임시로 일단 여기에 호출

        this.gameObject.SetActive(true);
    }


    public void Set_CurrRoomInfo()
    {
        roomNameText.text = PhotonLogicHandler.CurrentRoomName;

        Set_CurrMapInfo((ENUM_MAP_TYPE)Enum.Parse(typeof(ENUM_CHARACTER_TYPE), PhotonLogicHandler.CurrentMapName));
    }

    public void Set_CurrMapInfo(ENUM_MAP_TYPE _mapType)
    {
        // 맵 정보 갱신, 셋팅
    }

    public void ExitRoom()
    {
        bool isExit = PhotonLogicHandler.Instance.TryLeaveRoom(Close);
        if (!isExit)
            Managers.UI.popupCanvas.Open_NotifyPopup("방에서 나가지 못했습니다.");
    }

    private void Close()
    {
        isInit = false;

        // 이때 방에서 나가진 후에 Close 함수에서 자신의 프로필을 초기화를 시키는데
        // 이게 브로드캐스트가 될리가 없지 않나..?
        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI>
            (myProfile, myProfile.Clear);

        myProfile.isMine = false;
        this.gameObject.SetActive(false);
    }

    private void MasterClientSetting()
    {
        startButtonObject.SetActive(true);
        readyButtonObject.SetActive(false);
    }

    private void SlaveClientSetting()
    {
        readyButtonObject.SetActive(true);
        startButtonObject.SetActive(false);
    }
    public void OnClick_ChangeMap()
    {
        // 함수 두개로 나눌지, 하나의 함수에 인자를 줄지 고민중
    }

    public void OnClick_ExitRoom()
    {
        if(!PhotonLogicHandler.IsMasterClient)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI, bool>
            (myProfile, myProfile.Sync_ReadyStateImage, false);
        }

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "정말 방에서 나가시겠습니까?");
    }

    public void OnClick_Ready()
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            Debug.Log("마스터클라이언트가 준비버튼을 눌렀다?");
            return;
        }

        myProfile.OnClick_Ready();
    }

    public void OnClick_Start()
    {
        if (!PhotonLogicHandler.IsMasterClient)
        {
            Debug.Log("마스터클라이언트가 아닌데 시작버튼을 눌렀다?");
            return;
        }

        Debug.Log("시작하는 조건 확인하고, 배틀 씬으로 같이 이동 (미구현)");
    }
}
