using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchingUI : MonoBehaviour
{
    [SerializeField] CustomRoomListWindowUI customRoomListWindow;
    [SerializeField] CreateRoomWindowUI createRoomWindow;
    [SerializeField] CustomRoomWindowUI customRoomWindow;

    private void OnEnable()
    {
        Open_CustomRoom();
    }

    private void OnDisable()
    {
        
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
        PhotonLogicHandler.Instance.TryLeaveLobby(Close_CallBack);
    }

    private void Close_CallBack()
    {
        if (customRoomWindow.gameObject.activeSelf)
            customRoomWindow.Close();

        if (customRoomListWindow.gameObject.activeSelf)
            customRoomListWindow.Close();

        if (createRoomWindow.gameObject.activeSelf)
            createRoomWindow.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    private void Open_CustomRoom()
    {
        if (createRoomWindow.gameObject.activeSelf)
            createRoomWindow.gameObject.SetActive(false);

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
