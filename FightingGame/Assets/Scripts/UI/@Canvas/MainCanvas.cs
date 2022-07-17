using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainCanvas : BaseCanvas
{
    [SerializeField] ButtonUI buttonUI;
    [SerializeField] WindowUI windowUI;

    public override void Open<T>(UIParam param = null) // UIMgr.Open<> 세분화
    {
        if (typeof(T) == typeof(ButtonUI)) buttonUI.Open();
        else if (typeof(T) == typeof(WindowUI)) windowUI.Open();
        else if (typeof(T) == typeof(MatchWindow)) windowUI.OpenWindow<MatchWindow>();
        else if (typeof(T) == typeof(RankWindow)) windowUI.OpenWindow<RankWindow>();
        else if (typeof(T) == typeof(CharacterWindow)) windowUI.OpenWindow<CharacterWindow>();
        else if (typeof(T) == typeof(SettingWindow)) windowUI.OpenWindow<SettingWindow>();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>() // UIMgr.Close<> 세분화
    {
        Debug.Log(typeof(T));
        if (typeof(T) == typeof(ButtonUI)) buttonUI.Close();
        else if (typeof(T) == typeof(WindowUI)) windowUI.Close();
        else if (typeof(T) == typeof(MatchWindow)) windowUI.CloseWindow<MatchWindow>();
        else if (typeof(T) == typeof(RankWindow)) windowUI.CloseWindow<RankWindow>();
        else if (typeof(T) == typeof(CharacterWindow)) windowUI.CloseWindow<CharacterWindow>();
        else if (typeof(T) == typeof(SettingWindow)) windowUI.CloseWindow<SettingWindow>();
        else Debug.Log("범위 벗어남");
    }
}
