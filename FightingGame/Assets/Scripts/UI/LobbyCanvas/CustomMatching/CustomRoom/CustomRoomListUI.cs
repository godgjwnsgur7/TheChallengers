using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomRoomListUI : MonoBehaviour
{ 
    List<RoomListElementUI> roomListElements = new List<RoomListElementUI>();

    [SerializeField] GameObject noneRoomTextObject;
    [SerializeField] GameObject content;
    
    RectTransform contentRectTransform;
    ContentSizeFitter contentSizeFitter;

    bool isRoomUpdateLock = false;

    private void Awake()
    {
        contentRectTransform = content.GetComponent<RectTransform>();
        contentSizeFitter = content.GetComponent<ContentSizeFitter>();
    }

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

        if (customRoomList.Count >= 5)
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        else
        {
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentRectTransform.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
        }

        if (customRoomList.Count <= 0)
        {
            noneRoomTextObject.SetActive(true);
            gameObject.SetActive(true);
            return;
        }

        // 룸리스트 널체크
        for(int i = roomListElements.Count - 1; i >= 0; i--)
            if(roomListElements[i] == null)
                roomListElements.RemoveAt(i);

        // 커스텀룸의 정보 갯수보다 생성되어 있는 룸 갯수가 적을 때 차이만큼 생성
        if (customRoomList.Count > roomListElements.Count)
            for (int i = 0; i < customRoomList.Count - roomListElements.Count; i++)
                roomListElements.Add(Managers.Resource.Instantiate("UI/RoomListElement", content.transform).GetComponent<RoomListElementUI>());

        int roomIndex = customRoomList.Count;

        // 표시할 방의 갯수만큼 Open.
        for (int i = 0; i < roomListElements.Count; i++)
        {
            int customRoomIndex = 0;

            if (roomIndex > 0)
            {
                roomListElements[i].Open(customRoomList[customRoomIndex], Request_UpdateRoomList);
                customRoomIndex++;
                roomIndex--;
            }
            else
                roomListElements[i].Close();
        }

        noneRoomTextObject.SetActive(false);
        gameObject.SetActive(true);
    }
}