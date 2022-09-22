using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

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

    public void Open(CustomRoomInfo _roomInfo)
    {
        if (_roomInfo == null || isUsing)
        {
            Debug.LogError("roomInfo is Null or isUsing True");
            return;
        }

        isUsing = true;

        Set_CustomRoomInfo(_roomInfo);

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        isUsing = false;
    }

    public void Set_CustomRoomInfo(CustomRoomInfo _roomInfo)
    {
        // Set Texts
        personnelText.text = $"{_roomInfo.currentPlayerCount} / 2";
        roomNameText.text = _roomInfo.roomName;
        masterNicknameText.text = _roomInfo.masterClientNickname;

        // Set Image
        Set_MapImage(_roomInfo.currentMapType);

        if (_roomInfo.currentPlayerCount == 1)
            personnelImage.sprite = personnel_NoneSprite;
        else if (_roomInfo.currentPlayerCount == 2)
            personnelImage.sprite = personnel_ExistSprite;
        else
            Debug.LogError($"currentPlayerCount 값 오류 : {_roomInfo.currentPlayerCount}");
    }

    public void Set_MapImage(ENUM_MAP_TYPE _mapType)
    {

    }

    public void OnClick_JoinRoom()
    {
        if(!isUsing)
        {
            Debug.LogError("해당 로그가 떴다면, 깊히 반성해야 함");
            return;
        }

        // 이 때, 방 정보를 임시로 받아와서 저장해야 함

        // 취소 시에 임시로 받은 방 정보를 없애줄 필요가 있지 않을까?
        Managers.UI.popupCanvas.Open_SelectPopup(JoinRoom, null, "방에 입장하시겠습니까?");
        
    }

    public void JoinRoom()
    {
        // 방에 입장해야 함
    }
}
