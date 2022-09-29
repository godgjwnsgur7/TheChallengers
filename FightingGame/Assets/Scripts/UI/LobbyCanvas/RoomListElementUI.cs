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
    public CustomRoomInfo myRoomInfo;

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

        Show_MyRoomInfo();

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        isUsing = false;
    }

    public bool Update_MyRoomInfo()
    {
        myRoomInfo = PhotonLogicHandler.GetRoomInfo(roomNameText.text);

        if(myRoomInfo == null) // 방이 사라짐
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("없는 방입니다.", OnUpdateRoomList);
            Close();
            return false;
        }

        Show_MyRoomInfo();
        return true;
    }

    /// <summary>
    /// 호출 전에 예외처리 필요
    /// </summary>
    public void Show_MyRoomInfo()
    {
        if (myRoomInfo == null)
        {
            Debug.Log("myRoomInfo is Null!");
            return;
        }

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
            Debug.Log($"currentPlayerCount 값 오류 : {myRoomInfo.currentPlayerCount}");
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

        if (Update_MyRoomInfo()) // 이때 마이룸이 어딨어 이 미친놈아!
            return;

        Managers.UI.popupCanvas.Open_SelectPopup(JoinRoom, null, "방에 입장하시겠습니까?");
    }

    public void JoinRoom()
    {
        if (Update_MyRoomInfo())
            return;

        if (myRoomInfo.currentPlayerCount == 1)
        {
            // 방에 입장시키고 동기화처리 시작
        }
        else
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방에 자리가 없습니다.", OnUpdateRoomList);
        }
    }
}
