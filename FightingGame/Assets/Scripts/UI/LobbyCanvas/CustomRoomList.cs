using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 룸 갱신 정보를 받을 클래스에게 ILobbyPostProcess 인터페이스 상속
/// </summary>
public class CustomRoomList : MonoBehaviour, ILobbyPostProcess
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

	private void Awake()
	{
        // 등록해야 콜백을 받을 수 있다.
        this.RegisterLobbyCallback();
	}

	private void OnDestroy()
	{
        // 반드시 해제할 것
        this.UnregisterLobbyCallback();
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