using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class LobbyScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Lobby;
    }

    public override void Clear()
    {

    }
}
