using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomRoomListUI : MonoBehaviour
{ 
    List<RoomListElementUI> roomListElement = new List<RoomListElementUI>();
    
    [SerializeField] GameObject noneRoomTextObject;

    bool isRoomUpdateLock = false;

    public void Request_UpdateRoomList()
    {
        if (isRoomUpdateLock)
            return;

        isRoomUpdateLock = true;
        PhotonLogicHandler.Instance.RequestRoomList();

    }

	public void Update_RoomList(List<CustomRoomInfo> customRoomList)
    {
        gameObject.SetActive(false);

        // 커스텀룸의 정보 갯수보다 생성되어 있는 룸 갯수가 적을 때 차이만큼 생성
        if (customRoomList.Count > roomListElement.Count)
            for(int i = 0; i < customRoomList.Count - roomListElement.Count; i++)
                roomListElement.Add(Managers.Resource.Instantiate("UI/RoomListElement", this.transform).GetComponent<RoomListElementUI>());

        // 모든 방을 Close.
        for (int i = 0; i < roomListElement.Count; i++)
            roomListElement[i].Close();

        if (customRoomList.Count <= 0) 
        {
            noneRoomTextObject.SetActive(true);
            gameObject.SetActive(true);
            return;
        }

        // 현재 생성되어 있는 방의 갯수만큼 Open.
        for (int i = 0; i < customRoomList.Count; i++)
        {
            roomListElement[i].Open(customRoomList[i], Request_UpdateRoomList);
        }

        noneRoomTextObject.SetActive(false);
        gameObject.SetActive(true);
    }

}