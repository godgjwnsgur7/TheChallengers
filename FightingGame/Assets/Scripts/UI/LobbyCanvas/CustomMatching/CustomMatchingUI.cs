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

        if(PhotonLogicHandler.IsJoinedRoom)
        {
            if (customRoomListWindow.gameObject.activeSelf)
                customRoomListWindow.Close();

            customRoomWindow.Open();
        }
        else
        {
            if (customRoomWindow.gameObject.activeSelf)
                customRoomWindow.Close();

            customRoomListWindow.Open();
        }
    }

    public void Close()
    {
        if (customRoomWindow.gameObject.activeSelf)
            customRoomWindow.Close();

        if (customRoomListWindow.gameObject.activeSelf)
            customRoomListWindow.Close();

        this.gameObject.SetActive(false);
    }
}
