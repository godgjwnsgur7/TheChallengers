using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : BaseCanvas
{
    bool isLogin = true;
    [SerializeField] InteractableUI interactableUI;

    public override void Close<T>()
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Open();
        else Debug.Log("범위 벗어남");
    }

    public void OnClickStart()
    {
        if (isLogin)
        {
            Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Main);
        }
        else
        {
            if (interactableUI.gameObject.activeSelf)
            {
                Close<InteractableUI>();
            }
            else
            {
                Open<InteractableUI>();
            }
        }
    }
}
