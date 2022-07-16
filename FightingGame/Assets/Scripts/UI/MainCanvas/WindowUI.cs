using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUI : UIElement
{
    [SerializeField] RankWindow rankWindow;
    [SerializeField] CharacterWindow characterWindow;
    [SerializeField] SettingWindow settingWindow;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        CloseWindow();
        base.Close();
    }

    public void CloseWindow()
    {
        if (rankWindow.gameObject.activeSelf)
            rankWindow.Close();
        else if (characterWindow.gameObject.activeSelf)
            characterWindow.Close();
        else if (settingWindow.gameObject.activeSelf)
            settingWindow.Close();
    }

    public void OpenWindow<T>()
    {
        if (typeof(T) == typeof(RankWindow))
            rankWindow.Open();
        else if (typeof(T) == typeof(CharacterWindow))
            characterWindow.Open();
        else if (typeof(T) == typeof(SettingWindow))
            settingWindow.Open();
    }
}
