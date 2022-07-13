using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : BaseCanvas
{
    [SerializeField] MainButtonUI buttonUI;
    [SerializeField] MainPopupUI popupUI;

    public enum ENUM_BUTTON_TYPE 
    {
        Matching = 1,
        Custom = 2,
        Ranking = 3,
        Character = 4,
        Setting = 5,
    }

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(MainButtonUI)) buttonUI.Open();
        else if (typeof(T) == typeof(MainPopupUI)) popupUI.Open();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(MainButtonUI)) buttonUI.Close();
        else if (typeof(T) == typeof(MainPopupUI)) popupUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public void OnClickButton(int btnInt) {
        ENUM_BUTTON_TYPE btnType = (ENUM_BUTTON_TYPE)btnInt;

        switch (btnType) 
        {
            case ENUM_BUTTON_TYPE.Matching:
                Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
                break;
            case ENUM_BUTTON_TYPE.Custom:
                break;
            case ENUM_BUTTON_TYPE.Ranking:
            case ENUM_BUTTON_TYPE.Character:
            case ENUM_BUTTON_TYPE.Setting:
                Open<MainPopupUI>();
                popupUI.OpenPane((int)btnType);
                break;
        }
    }

    public void OnClickStart()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
