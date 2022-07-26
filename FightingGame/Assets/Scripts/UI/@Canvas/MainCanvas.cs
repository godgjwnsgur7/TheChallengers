using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : BaseCanvas
{
    bool isLogin = true;
    [SerializeField] InteractableUI interactableUI;
    [SerializeField] ProduceUI produceUI;

    public override void Close<T>()
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Close();
        else if (typeof(T) == typeof(ProduceUI)) produceUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Open();
        else if (typeof(T) == typeof(ProduceUI)) produceUI.Open();
        else Debug.Log("범위 벗어남");
    }

    public void OnClickStart()
    {
        if (isLogin)
        {
            Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
        }
        else
        {
            if (interactableUI.gameObject.activeSelf)
            {
                Managers.UI.CloseUI<InteractableUI>();
            }
            else
            {
                Managers.UI.OpenUI<InteractableUI>();
            }
        }
    }

    public void OnClickProduce() 
    {
        if (produceUI.gameObject.activeSelf)
            Managers.UI.CloseUI<ProduceUI>();
        else
            Managers.UI.OpenUI<ProduceUI>();
    }
}
