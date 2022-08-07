using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerErrorPopup1 : PopupUI
{
    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        Managers.UI.CloseUI<ServerErrorPopup1>();

        // PlayerPrefs 지우는 작업...

        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
