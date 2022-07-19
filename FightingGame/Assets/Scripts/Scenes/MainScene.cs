using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    PlatformAuth auth = new PlatformAuth();

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Main;
    }

    public void SignIn(ENUM_LOGIN_TYPE loginType, string email, string password)
    {
        bool isFirstConnect = auth.TryConnectAuth(OnConnectAuthSuccess: () =>
        {
            auth.SignIn(loginType, email, password);
        });

        if (!isFirstConnect)
        {
            auth.SignIn(loginType, email, password);
        }
    }

    public override void Clear()
    {

    }
}
