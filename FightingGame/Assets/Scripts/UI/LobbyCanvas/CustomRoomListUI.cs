using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 룸 갱신 정보를 받을 클래스에게 ILobbyPostProcess 인터페이스 상속
/// </summary>
public class CustomRoomListUI : MonoBehaviour, ILobbyPostProcess
{
    List<RoomListElementUI> roomList = new List<RoomListElementUI>();
    List<CustomRoomInfo> roomInfoList = new List<CustomRoomInfo>();

    [SerializeField] GameObject noneRoomTextObject;

    bool isUpdating = false;
    bool isRegisting = false;

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

    public void Register_LobbyCallback()
    {
        if (isRegisting)
            Debug.Log("이미 등록된 상태");

        isRegisting = true;
        this.RegisterLobbyCallback();
    }
    public void UnRegister_LobbyCallback()
    {
        if (!isRegisting)
            Debug.LogError("등록되지 않은 상태에서 해제를 시도함");

        isRegisting = false;
        this.UnRegister_LobbyCallback();
    }

    /// <summary>
    /// 로비 내 룸이 갱신될 때마다 해당 콜백이 불림
    /// </summary>
    /// <param name="roomList"></param>

    public void OnUpdateLobby(List<CustomRoomInfo> roomList)
	{
        roomInfoList = roomList;
    }

	public void Update_RoomList()
    {
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
            return;
        }

        // 현재 생성되어 있는 방의 갯수만큼 Open.
        for (int i = 0; i < roomInfoList.Count; i++)
            roomList[i].Open(roomInfoList[i]);

        noneRoomTextObject.SetActive(false);
    }
}