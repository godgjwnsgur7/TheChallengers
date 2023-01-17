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
        if (Managers.Battle.isCustom)
        {
            LobbyCanvas lobbyCanvas = Managers.UI.currCanvas.GetComponent<LobbyCanvas>();
            lobbyCanvas.Open_CustomMatchingWindow();
        }
        else if (PhotonLogicHandler.IsJoinedRoom)
        {
            PhotonLogicHandler.Instance.TryLeaveRoom();
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
