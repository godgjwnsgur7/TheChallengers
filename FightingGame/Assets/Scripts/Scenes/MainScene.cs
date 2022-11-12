using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MainScene : BaseScene
{
    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Main;

        base.Init();
    }

    public override void Clear()
    {
        base.Clear();
    }

    public override void Update_BGM()
    {
        Managers.Sound.Play(ENUM_BGM_TYPE.TestBGM, ENUM_SOUND_TYPE.BGM);
    }
}
