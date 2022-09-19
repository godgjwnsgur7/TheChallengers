using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomWindow : MonoBehaviour
{
    public CustomRoomList customRoomList;

    private void OnEnable()
    {
        OnClick_GetLobbyList();
    }

    public void OnClick_GetLobbyList()
    {
        customRoomList.Get_LobbyList();
    }
}
