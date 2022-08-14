using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public override void Init()
    {
        /*if (유저 로그인 상태가 아닐때)
        {
            Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Main);
        }*/

        base.Init();

        SceneType = ENUM_SCENE_TYPE.Lobby;
    }

    public override void Clear()
    {

    }
}
