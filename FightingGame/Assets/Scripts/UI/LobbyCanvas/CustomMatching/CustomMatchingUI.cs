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

        if (createRoomWindow.gameObject.activeSelf)
            createRoomWindow.Close();

        bool isLeaveLobby = PhotonLogicHandler.Instance.TryLeaveLobby();
        this.gameObject.SetActive(false);

        if(!isLeaveLobby)
        {
            Debug.Log("왜 실패했냐 ㅋㅋㅋ");
        }
        
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
