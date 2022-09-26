using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class RoomListElementUI : MonoBehaviour
{
    public bool isUsing = false;

    [Header ("Set In Editor")]
    [SerializeField] Image MapImage;
    [SerializeField] Text roomNameText;
    [SerializeField] Text masterNicknameText;

    [SerializeField] Image personnelImage;
    [SerializeField] Text personnelText;

    [Header("Setting Resources With Editor")]
    [SerializeField] Sprite personnel_NoneSprite;
    [SerializeField] Sprite personnel_ExistSprite;

    Action OnUpdateRoomList = null;
    CustomRoomInfo myRoomInfo;

    public void Open(CustomRoomInfo _roomInfo, Action _OnUpdateRoomList)
    {
        if (_roomInfo == null || isUsing)
        {
            Debug.LogError("roomInfo is Null or isUsing True");
            return;
        }

        isUsing = true;

        myRoomInfo = _roomInfo;
        OnUpdateRoomList = _OnUpdateRoomList;

        Set_CustomRoomInfo();

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        isUsing = false;
    }

    public void Update_MyRoomInfo()
    {
        int roomId = myRoomInfo.roomId;
        myRoomInfo = PhotonLogicHandler.GetRoomInfo(roomId);

        if(myRoomInfo == null) // 방이 사라짐
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("없는 방입니다.", OnUpdateRoomList);
            Close();
            return;
        }

        Set_CustomRoomInfo();
    }

    public void Set_CustomRoomInfo()
    {
        // Set Texts
        personnelText.text = $"{myRoomInfo.currentPlayerCount} / 2";
        roomNameText.text = myRoomInfo.roomName;
        masterNicknameText.text = myRoomInfo.masterClientNickname;

        // Set Image
        Set_MapImage(myRoomInfo.currentMapType);

        if (myRoomInfo.currentPlayerCount == 1)
            personnelImage.sprite = personnel_NoneSprite;
        else if (myRoomInfo.currentPlayerCount == 2)
            personnelImage.sprite = personnel_ExistSprite;
        else
            Debug.LogError($"currentPlayerCount 값 오류 : {myRoomInfo.currentPlayerCount}");
    }

    public void Set_MapImage(ENUM_MAP_TYPE _mapType)
    {
        // 맵 정보 받아와서 갱신
    }

    public void OnClick_JoinRoom()
    {
        if(!isUsing)
        {
            Debug.LogError("해당 로그가 떴다면, 깊히 반성해야 함");
            return;
        }

        Update_MyRoomInfo();

        // 취소 시에 임시로 받은 방 정보를 없애줄 필요가 있지 않을까?
        Managers.UI.popupCanvas.Open_SelectPopup(JoinRoom, null, "방에 입장하시겠습니까?");
    }

    public void JoinRoom()
    {
        Update_MyRoomInfo();

        if (myRoomInfo.currentPlayerCount == 1)
        {
            // 방에 입장
        }
        else
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방에 자리가 없습니다.", OnUpdateRoomList);
        }
    }
}
