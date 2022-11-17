using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomListWindowUI : MonoBehaviour, ILobbyPostProcess
{
    [SerializeField] CustomRoomListUI customRoomList;

    List<CustomRoomInfo> customRoomInfoList = new List<CustomRoomInfo>();

    bool isRoomUpdateLock = false;

    readonly float roomUpdateCoolTime = 1.0f;

    private void OnEnable()
    {
        this.RegisterLobbyCallback();

        Request_UpdateRoomList();
    }

    private void OnDisable()
    {
        this.UnregisterLobbyCallback();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 로비 내 룸이 갱신될 때마다 해당 콜백이 불림
    /// </summary>
    public void OnUpdateLobby(List<CustomRoomInfo> roomList)
    {
        customRoomInfoList.Clear();

        // 룸 정보 중에 커스텀 룸의 정보만 뽑아서 따로 담음
        for (int i = 0; i < roomList.Count; i++)
            if (roomList[i].IsCustom)
                customRoomInfoList.Add(roomList[i]);

        customRoomList.Update_RoomList(customRoomInfoList);
    }

    public void Request_UpdateRoomList()
    {
        if (isRoomUpdateLock)
            return;

        isRoomUpdateLock = true;
        PhotonLogicHandler.Instance.RequestRoomList();

        StartCoroutine(IUpdateLockTime());
    }

    protected IEnumerator IUpdateLockTime()
    {
        yield return new WaitForSeconds(roomUpdateCoolTime);

        isRoomUpdateLock = false;
    }
}
