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

        if (PhotonLogicHandler.IsJoinedRoom)
        {
            if(PhotonLogicHandler.IsMasterClient)
                Managers.Network.Start_SequenceExecuter();
            
            Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_CustomMatchingWindow();
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
