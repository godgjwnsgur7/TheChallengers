using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Lobby;
    }

    public override void Clear()
    {
        
    }

}
