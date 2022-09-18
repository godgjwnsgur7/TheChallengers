using System.Collections;
using UnityEngine;
using FGDefine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Login;
    }

    public override void Clear()
    {
        
    }
}
