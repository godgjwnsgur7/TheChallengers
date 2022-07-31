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
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.CustomRoom);
    }
}
