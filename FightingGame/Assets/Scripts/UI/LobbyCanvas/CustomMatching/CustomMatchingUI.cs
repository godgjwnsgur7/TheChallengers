using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchingUI : MonoBehaviour
{
    [SerializeField] CustomRoomListWindowUI customRoomListWindow;
    [SerializeField] CustomRoomWindowUI customRoomWindow;

    public void Open()
    {
        gameObject.SetActive(true);

        customRoomListWindow.Open();

        if (customRoomWindow.gameObject.activeSelf)
            customRoomWindow.Close();
    }

    public void Close()
    {
        if (customRoomWindow.gameObject.activeSelf)
            customRoomWindow.Close();

        if (customRoomListWindow.gameObject.activeSelf)
            customRoomListWindow.Close();

        this.gameObject.SetActive(false);
    }

    public void Open_CustomRoomWindow()
    {
        customRoomListWindow.Open();

        if (customRoomListWindow.gameObject.activeSelf)
            customRoomListWindow.Close();
    }

    public void Set_InTheCustomRoom()
    {
        PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
        customRoomWindow.Open();

        if (customRoomListWindow.gameObject.activeSelf)
            customRoomListWindow.Close();
    }
}
