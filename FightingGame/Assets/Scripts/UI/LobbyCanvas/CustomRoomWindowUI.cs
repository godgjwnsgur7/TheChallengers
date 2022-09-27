using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

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

        isInit = true;

        base.Init();
    }

    public void Open()
    {
        Init();

        this.gameObject.SetActive(true);
    }

    public void Set_CurrRoomInfo()
    {
        // 음?

        roomNameText.text = PhotonLogicHandler.CurrentRoomName;

        PhotonLogicHandler.Instance.TryBroadcastMethod<CustomRoomWindowUI>(this, Set_CurrMapInfo);
    }

    [BroadcastMethod]
    public void Set_CurrMapInfo()
    {
        // 맵 정보 갱신, 셋팅
    }

    public void ExitRoom()
    {
        // 이때 방에서 나가진 후에 Close 함수에서 자신의 프로필을 초기화를 시키는데
        // 이게 될리가 없는걸?...ㅋ
        bool isExit = PhotonLogicHandler.Instance.TryLeaveRoom(Close);
        if (!isExit)
            Managers.UI.popupCanvas.Open_NotifyPopup("방에서 나가지 못했습니다.");
    }

    private void Close()
    {
        isInit = false;

        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI>
            (myProfile, myProfile.Clear);

        this.gameObject.SetActive(false);
    }

    public void OnClick_ChangeMap()
    {
        // 함수 두개로 나눌지, 하나의 함수에 인자를 줄지 고민중
    }

    public void OnClick_ExitRoom()
    {
        // 누르자 마자 준비해제?

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
        if (PhotonLogicHandler.IsMasterClient)
        {
            Debug.Log("마스터클라이언트가 아닌데 시작버튼을 눌렀다?");
            return;
        }

        // 배틀 씬으로 같이 이동.?
    }
}
