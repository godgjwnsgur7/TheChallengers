using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기서 CustomRoom 디비랑 소통해야?

public class CustomRoomList : MonoBehaviour
{
    RoomListElement[] roomListElements;

    public void UpdateLobbyList()
    {
        Debug.Log("로비리스트 받아와 체크");
    }
}
