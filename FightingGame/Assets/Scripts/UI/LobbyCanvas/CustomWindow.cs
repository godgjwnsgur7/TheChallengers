using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWindow : MonoBehaviour
{
    public CustomRoomList customRoomList;

    private void OnEnable()
    {
        OnClick_UpdateList();
    }

    public void OnClick_UpdateList()
    {
        customRoomList.UpdateLobbyList();
    }
}
