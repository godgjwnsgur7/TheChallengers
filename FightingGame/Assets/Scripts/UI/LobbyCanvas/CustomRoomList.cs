using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomList : MonoBehaviour
{
    public RoomListElement[] roomListElements = new RoomListElement[1];

    public void UpdateLobbyList() // 새로고침 연타 막아야함
    {
        Debug.Log("로비리스트 받아와서 생성");
        // Destroy_RoomList();
        Create_RoomListElements();
    }

    public void Destroy_RoomList()
    {
        for(int i = 0; i < roomListElements.Length; i++)
        {
            Destroy(roomListElements[i].gameObject);
        }
    }

    public void Create_RoomListElements()
    {
        // 요런 느낌?

        roomListElements[0] = Managers.Resource.Instantiate("UI/RoomListElement", this.transform).gameObject.GetComponent<RoomListElement>();
        // roomListElements[0].Init()
    }

}
