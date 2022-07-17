using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUI : UIElement
{
    [SerializeField] MatchWindow matchwindow;
    [SerializeField] RankWindow rankWindow;
    [SerializeField] CharacterWindow characterWindow;
    [SerializeField] SettingWindow settingWindow;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void CloseWindow<T>()
    {
        if (typeof(T) == typeof(MatchWindow))
            matchwindow.Close();
        else if (typeof(T) == typeof(RankWindow))
            rankWindow.Close();
        else if (typeof(T) == typeof(CharacterWindow))
            characterWindow.Close();
        else if (typeof(T) == typeof(SettingWindow))
            settingWindow.Close();
    }

    public void OpenWindow<T>()
    {
        if (typeof(T) == typeof(MatchWindow))
            matchwindow.Open();
        else if (typeof(T) == typeof(RankWindow))
            rankWindow.Open();
        else if (typeof(T) == typeof(CharacterWindow))
            characterWindow.Open();
        else if (typeof(T) == typeof(SettingWindow))
            settingWindow.Open();
    }
}
