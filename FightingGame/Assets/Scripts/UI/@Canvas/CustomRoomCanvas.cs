using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomCanvas : BaseCanvas
{
    public override void Open<T>(UIParam param = null)
    {

    }

    public override void Close<T>()
    {

    }

    public void LoadLobby() 
    {
        Debug.Log("작동");
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
