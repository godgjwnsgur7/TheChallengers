using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomRoomList : MonoBehaviour
{
    List<RoomListElement> roomList = new List<RoomListElement>();
    List<CustomRoomInfo> roomInfoList = new List<CustomRoomInfo>();

    [SerializeField] GameObject noneRoomTextObject;

    bool isUpdating = false;

    public void Get_CustomRoomList()
    {
        if (isUpdating) return;

        isUpdating = true;

        roomInfoList = PhotonLogicHandler.AllRoomInfos;

        Debug.Log($"roomInfoListCount : {roomInfoList.Count} ");

        gameObject.SetActive(false);
        // Open Loading UI (추가사항)

        Update_RoomList();

        // Close Loading UI (추가사항)
        gameObject.SetActive(true);

        isUpdating = false;
    }

    public void Update_RoomList()
    {
        // 룸 정보 갯수보다 생성되어 있는 룸 갯수가 적을 때 차이만큼 생성
        if (roomInfoList.Count > roomList.Count)
            for(int i = 0; i < roomInfoList.Count - roomList.Count; i++)
                roomList.Add(Managers.Resource.Instantiate("UI/RoomListElement", this.transform).GetComponent<RoomListElement>());

        // 모든 방을 Close.
        for (int i = 0; i < roomList.Count; i++)
            roomList[i].Close();

        if (roomInfoList.Count <= 0) 
        {
            noneRoomTextObject.SetActive(true);
            return;
        }

        // 현재 생성되어 있는 방의 갯수만큼 Open.
        for (int i = 0; i < roomInfoList.Count; i++)
            roomList[i].Open(roomInfoList[i]);

        noneRoomTextObject.SetActive(false);
    }
}