using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomRoomListUI : MonoBehaviour, ILobbyPostProcess
{
    List<RoomListElementUI> roomList = new List<RoomListElementUI>();
    List<CustomRoomInfo> roomInfoList = new List<CustomRoomInfo>();

    [SerializeField] CustomRoomWindowUI customRoomWindow;
    [SerializeField] GameObject noneRoomTextObject;

    bool isUpdateLock = false;
    bool isLobbyRegisting = false;

    public void Get_CustomRoomList()
    {
        if (isUpdateLock) return;

        isUpdateLock = true;

        PhotonLogicHandler.Instance.RequestRoomList();

        StartCoroutine(IUpdateLockTime(1.0f));        
    }

    public bool Register_LobbyCallback()
    {
        if (isLobbyRegisting)
        {
            Debug.Log("이미 등록된 상태");
            return false;
        }

        isLobbyRegisting = true;
        this.RegisterLobbyCallback();
        return true;
    }
    public void UnRegister_LobbyCallback()
    {
        if (!isLobbyRegisting)
            return;

        isLobbyRegisting = false;
        this.UnregisterLobbyCallback();
    }

    /// <summary>
    /// 로비 내 룸이 갱신될 때마다 해당 콜백이 불림
    /// </summary>
    public void OnUpdateLobby(List<CustomRoomInfo> roomList)
	{
        roomInfoList = roomList;

        Debug.Log($"roomInfoListCount : {roomInfoList.Count} ");
        
        Update_RoomList();
    }

	public void Update_RoomList()
    {
        gameObject.SetActive(false);

        // 룸 정보 갯수보다 생성되어 있는 룸 갯수가 적을 때 차이만큼 생성
        if (roomInfoList.Count > roomList.Count)
            for(int i = 0; i < roomInfoList.Count - roomList.Count; i++)
                roomList.Add(Managers.Resource.Instantiate("UI/RoomListElement", this.transform).GetComponent<RoomListElementUI>());

        // 모든 방을 Close.
        for (int i = 0; i < roomList.Count; i++)
            roomList[i].Close();

        if (roomInfoList.Count <= 0) 
        {
            noneRoomTextObject.SetActive(true);
            gameObject.SetActive(true);
            return;
        }

        // 현재 생성되어 있는 방의 갯수만큼 Open.
        for (int i = 0; i < roomInfoList.Count; i++)
        {
            roomList[i].Open(roomInfoList[i], Get_CustomRoomList, Active_CustomRoomWindow);
        }

        noneRoomTextObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Active_CustomRoomWindow()
    {
        customRoomWindow.Open();
    }

    private IEnumerator IUpdateLockTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isUpdateLock = false;
    }
}