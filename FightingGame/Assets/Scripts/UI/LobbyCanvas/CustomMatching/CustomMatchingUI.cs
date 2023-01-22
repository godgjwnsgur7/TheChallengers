using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchingUI : MonoBehaviour
{
    [SerializeField] CustomRoomListWindowUI customRoomListWindow;
    [SerializeField] CustomRoomWindowUI customRoomWindow;

    private void OnEnable()
    {
        Open_CustomRoom();
    }

    public void Open()
    {
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
        else
        {
            Open_CustomRoom();
        }
    }

    public void Close()
    {
        if(PhotonLogicHandler.IsJoinedRoom)
        {
            PhotonLogicHandler.Instance.TryLeaveRoom(Close_CallBack);
        }
        else
        {
            Close_CallBack();
        }
    }

    private void Close_CallBack()
    {
        if (customRoomWindow.gameObject.activeSelf)
            customRoomWindow.Close();

        if (customRoomListWindow.gameObject.activeSelf)
            customRoomListWindow.Close();

        this.gameObject.SetActive(false);
        
    }

    private void Open_CustomRoom()
    {
        if (!PhotonLogicHandler.IsJoinedRoom)
        {
            customRoomListWindow.Open();

            if (customRoomWindow.gameObject.activeSelf)
                customRoomWindow.Close();
        }
        else
        {
            customRoomWindow.Open();

            if (customRoomListWindow.gameObject.activeSelf)
                customRoomListWindow.Close();
        }
    }
}
