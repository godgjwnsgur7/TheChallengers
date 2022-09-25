using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CustomRoomWindowUI : MonoBehaviour
{
    [SerializeField] Text roomNameText;

    [SerializeField] Image currMapIamge;
    [SerializeField] Image nextMapIamge_Left;
    [SerializeField] Image nextMapIamge_Right;

    [SerializeField] CharProfileUI masterProfile;
    [SerializeField] CharProfileUI slaveProfile;

    CharProfileUI myProfile;

    public bool isInit = false;

    public void Init()
    {
        if (isInit) return;

        isInit = true;
        // 현재 방 정보를 받아와야 함

        if (PhotonLogicHandler.IsMasterClient)
            myProfile = masterProfile;
        else
            myProfile = slaveProfile;
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
        myProfile.OnClick_Ready();
    }

    public void OnClick_Start()
    {
        // 배틀 씬으로 같이 이동.?
    }

    public void ExitRoom()
    {
        Clear();
        PhotonLogicHandler.Instance.TryLeaveRoom();
    }

    private void Clear()
    {

    }
}
