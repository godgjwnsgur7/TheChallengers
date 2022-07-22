using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] MatchWindow matchWindow;
    [SerializeField] RankWindow rankWindow;
    [SerializeField] CharacterWindow characterWindow;
    [SerializeField] SettingWindow settingWindow;
    [SerializeField] MatchTimeWindow matchTimeWindow;

    public override void Open<T>(UIParam param = null) // UIMgr.Open<> 세분화
    {
        if (typeof(T) == typeof(MatchWindow)) matchWindow.Open();
        else if (typeof(T) == typeof(RankWindow)) rankWindow.Open();
        else if (typeof(T) == typeof(CharacterWindow)) characterWindow.Open();
        else if (typeof(T) == typeof(SettingWindow)) settingWindow.Open();
        else if (typeof(T) == typeof(MatchTimeWindow)) matchTimeWindow.Open();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>() // UIMgr.Close<> 세분화
    {
        if (typeof(T) == typeof(MatchWindow)) matchWindow.Close();
        else if (typeof(T) == typeof(RankWindow)) rankWindow.Close();
        else if (typeof(T) == typeof(CharacterWindow)) characterWindow.Close();
        else if (typeof(T) == typeof(SettingWindow)) settingWindow.Close();
        else if (typeof(T) == typeof(MatchTimeWindow)) matchTimeWindow.Close();
        else Debug.Log("범위 벗어남");
    }

    public void OnWindowButton(string btnType) // Windows setActive(True) When Button Click 
    {
        switch (btnType) 
        {
            case "Match":
                Managers.UI.OpenUI<MatchWindow>();
                break;
            case "Rank":
                Managers.UI.OpenUI<RankWindow>();
                break;
            case "Character":
                Managers.UI.OpenUI<CharacterWindow>();
                break;
            case "Setting":
                Managers.UI.OpenUI<SettingWindow>();
                break;
        }
    }

    public void OffWindowButton(string btnType) // Windows SetActive(false) when CloseBtn Click
    {
        switch (btnType)
        {
            case "Match":
                Managers.UI.CloseUI<MatchWindow>();
                break;
            case "Rank":
                Managers.UI.CloseUI<RankWindow>();
                break;
            case "Character":
                Managers.UI.CloseUI<CharacterWindow>();
                break;
            case "Setting":
                Managers.UI.CloseUI<SettingWindow>();
                break;
        }
    }

    public void SelectCharacter(int charType) 
    {
        matchTimeWindow.charType = (ENUM_CHARACTER_TYPE)charType;
        Managers.UI.OpenUI<MatchTimeWindow>();
        Managers.UI.CloseUI<MatchWindow>();
    }
}
