using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : BaseCanvas
{
    public override void Open<T>()
    {

    }

    public override void Close<T>()
    {

    }

    public void OnClickStart()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Battle);
    }
}
