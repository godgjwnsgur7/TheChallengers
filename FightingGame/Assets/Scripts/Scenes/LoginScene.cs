using System.Collections;
using UnityEngine;
using FGDefine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    PlatformAuth auth = new PlatformAuth();

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Login;
    }

    public void SignIn(ENUM_LOGIN_TYPE loginType, string email, string password)
    {
        bool isFirstConnect = auth.TryConnectAuth(OnConnectAuthSuccess: () =>
        {
            auth.SignIn(loginType, email, password);
        });

        if(!isFirstConnect)
        {
            auth.SignIn(loginType, email, password);
        }
    }

    public override void Clear()
    {
        
    }
}
