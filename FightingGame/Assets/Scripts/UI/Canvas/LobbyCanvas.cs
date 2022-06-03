using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class LobbyCanvas : BaseCanvas
{
    [SerializeField] SelectionUI selectionUI;

    public override void Open<T>()
    {

    }

    public override void Close<T>()
    {

    }

    public void OnClickSeleteChar(ENUM_CHARACTER_TYPE type)
    {

    }

    public void OnClickStart()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Battle);
    }
}
