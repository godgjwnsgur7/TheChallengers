using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingWindow : UIElement
{
    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("LoginUser");
        
        // 마스터 서버 연결 해제할 수 있으면 좋을거같은데

        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Main);
    }
}
