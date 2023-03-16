using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class LobbyScene : BaseScene
{
    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Lobby;

        base.Init();
    }

    public override void Clear()
    {
        base.Clear();
    }

    public override void Play_BGM()
    {
        Managers.Sound.Play_BGM(ENUM_BGM_TYPE.Main);
    }
}
