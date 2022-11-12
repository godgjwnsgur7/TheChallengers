using System.Collections;
using UnityEngine;
using FGDefine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Login;

        base.Init();
    }

    public override void Clear()
    {
        
    }

    public override void Update_BGM()
    {
        Managers.Sound.Play(ENUM_BGM_TYPE.TestBGM, ENUM_SOUND_TYPE.BGM);
    }
}
