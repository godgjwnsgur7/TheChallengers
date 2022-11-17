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
        if (!PhotonLogicHandler.IsConnected)
            return;

        if (!PhotonLogicHandler.IsJoinedRoom)
        {
            customRoomListWindow.Open();
        }
        else
        {
            customRoomWindow.Open();
        }
    }

    public void Open()
    {
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
    }
}
