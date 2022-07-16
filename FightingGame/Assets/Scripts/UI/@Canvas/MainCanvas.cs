using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainCanvas : BaseCanvas
{
    [SerializeField] ButtonUI buttonUI;
    [SerializeField] WindowUI windowUI;

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(ButtonUI)) buttonUI.Open();
        else if (typeof(T) == typeof(WindowUI)) windowUI.Open();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(ButtonUI)) buttonUI.Close();
        else if (typeof(T) == typeof(WindowUI)) windowUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public void OnClickButton(string btnText)
    {
        switch (btnText)
        {
            case "Match":
                //Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
                SceneManager.LoadScene("Lobby");
                break;
            case "Custom":
                break;
            case "Rank":
                Open<WindowUI>();
                windowUI.OpenWindow<RankWindow>();
                break;
            case "Character":
                Open<WindowUI>();
                windowUI.OpenWindow<CharacterWindow>();
                break;
            case "Setting":
                Open<WindowUI>();
                windowUI.OpenWindow<SettingWindow>();
                break;
        }
    }

    public void OnClickStart()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
