using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomListPanel : UIElement
{
    private void OnEnable()
    {
        // 이때 서버에서 방목록을 불러오면 될거 같기도..?
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void EnterCustomScene()
    {
        // PlayerPrefs.SetString("RoomName", 선택한 방의 방제목);
        // PlayerPrefs.SetString("CreateUser", 선택한 방의 방장이름);
        // PlayerPrefs.SetString("MapSpriteP", 선택한 방의 맵);
        PlayerPrefs.SetString("EnterUser", "hjh");
        PlayerPrefs.SetString("MyName", "hjh");
        PlayerPrefs.SetString("MyTeam", "Red");
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.CustomRoom);
    }
}
