using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    public override void Open<T>(UIParam param = null)
    {
        // if (typeof(T) == typeof(MatchWindow)) matchWindow.Open();
        // else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        // if (typeof(T) == typeof(MatchWindow)) matchWindow.Close();
        // else Debug.Log("범위 벗어남");
    }

    public override T GetUIComponent<T>()
    {

        return default(T);
    }
}
