using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchingUI : MonoBehaviour
{
    public CustomRoomListUI customRoomList;
    public CustomRoomWindowUI customRoomWindow;

    bool isRegiserSuccess;

    private void OnEnable()
    {
        isRegiserSuccess = customRoomList.Register_LobbyCallback();

        if (!isRegiserSuccess)
        {
            gameObject.SetActive(false);
            Debug.Log("등록 실패");
            return;
        }

        OnClick_GetLobbyList();
    }

    private void OnDisable()
    {
        customRoomList.UnRegister_LobbyCallback();
    }

    public void OnClick_GetLobbyList()
    {
        customRoomList.Get_CustomRoomList();
    }
}
