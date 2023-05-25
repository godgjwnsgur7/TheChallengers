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
    [SerializeField] Image mapImage;
    [SerializeField] Text roomNameText;
    [SerializeField] Text masterNicknameText;

    [SerializeField] Text personnelText;

    Action OnUpdateRoomList = null;
    public CustomRoomInfo myRoomInfo;

    ENUM_MAP_TYPE currMap;
    ENUM_MAP_TYPE CurrMap
    {
        set { CurrMapInfoUpdateCallBack(value); }
    }

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

        myRoomInfo = null;
        OnUpdateRoomList = null;
        isUsing = false;
    }
    
    public void CurrMapInfoUpdateCallBack(ENUM_MAP_TYPE _mapType)
    {
        if (currMap == _mapType) return;
        currMap = _mapType;
        mapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
    }

    public bool Update_MyRoomInfo()
    {
        string roomName = roomNameText.text;
        myRoomInfo = PhotonLogicHandler.GetRoomInfo(roomName);

        if(myRoomInfo == null) // 방이 사라짐
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("없는 방입니다.", OnUpdateRoomList);
            Close();
            return false;
        }

        if(myRoomInfo.currentPlayerCount == 2)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방에 인원이 가득 찼습니다.", OnUpdateRoomList);
            Close();
            return false;
        }

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
        masterNicknameText.text = myRoomInfo.MasterClientNickname;

        // Set Image
        CurrMap = myRoomInfo.CurrentMapType;
    }

    public void OnClick_JoinRoom()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(JoinRoom, Show_MyRoomInfo, "방에 입장하시겠습니까?");
    }

    public void JoinRoom()
    {
        if (PhotonLogicHandler.IsJoinedRoom)
            return; 

        if (!PhotonLogicHandler.Instance.TryJoinRoom(JoinRoomSuccessCallBack, null, myRoomInfo.roomName))
            Managers.UI.popupCanvas.Open_NotifyPopup("방에 입장하지 못했습니다.", OnUpdateRoomList);
    }

    public void JoinRoomSuccessCallBack()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_CustomRoomWindow);
    }
}
