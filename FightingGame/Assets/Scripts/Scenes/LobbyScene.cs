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

        if(Managers.Battle.isCustom)
        {
            LobbyCanvas lobbyCanvas = Managers.UI.currCanvas.GetComponent<LobbyCanvas>();
            lobbyCanvas.Set_InTheCustomRoom();
        }
    }

    public override void Clear()
    {

    }
}
