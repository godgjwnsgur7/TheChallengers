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
    Action OnActiveRoomWindow = null;
    public CustomRoomInfo myRoomInfo;

    public void Open(CustomRoomInfo _roomInfo, Action _OnUpdateRoomList, Action _OnActiveRoomWindow)
    {
        if (_roomInfo == null || isUsing)
        {
            Debug.LogError("roomInfo is Null or isUsing True");
            return;
        }

        isUsing = true;

        myRoomInfo = _roomInfo;
        OnUpdateRoomList = _OnUpdateRoomList;
        OnActiveRoomWindow = _OnActiveRoomWindow;

        Show_MyRoomInfo();

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        myRoomInfo = null;
        OnUpdateRoomList = null;
        OnActiveRoomWindow = null;
        isUsing = false;
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

        Managers.UI.popupCanvas.Open_SelectPopup(JoinRoom, Show_MyRoomInfo, "방에 입장하시겠습니까?");
    }

    public void JoinRoom()
    {
        // 기존에 방을 참여한 다음 콜백으로 OnActiveRoomWindow를 수행하고 있지 않았음

        // 여기서 JoinRoom이 뱉는 리턴값은 성공 실패 여부이기는 한데,
        // 성공했다고 반드시 룸에 입장한 이후라고 보장할 수는 없기 때문에 콜백을 달아놓은 것임
        
        if (!PhotonLogicHandler.Instance.TryJoinRoom(OnActiveRoomWindow, null, myRoomInfo.roomName))
            Managers.UI.popupCanvas.Open_NotifyPopup("방에 입장하지 못했습니다.", OnUpdateRoomList);

    }
}
