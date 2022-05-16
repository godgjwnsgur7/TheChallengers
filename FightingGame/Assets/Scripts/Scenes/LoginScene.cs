using System.Collections;
using UnityEngine;
using FGDefine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    PlatformAuth auth = new PlatformAuth();

    protected override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Login;
    }

    public void SignIn(string email, string password)
    {
        auth.TryConnectAuth(OnConnectAuthSuccess: () =>
        {
            auth.SignInWithEmailAndPassword(email, password);
        });
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }
}
