using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] MatchWindow matchWindow;
    [SerializeField] CustomWindow customWindow;
    [SerializeField] RankWindow rankWindow;
    [SerializeField] CharacterWindow characterWindow;
    [SerializeField] TrainingWindow trainingWindow;
    [SerializeField] SettingWindow settingWindow;
    [SerializeField] MatchTimeWindow matchTimeWindow;

    public override void Open<T>(UIParam param = null) // UIMgr.Open<> 세분화
    {
        if (typeof(T) == typeof(MatchWindow)) matchWindow.Open();
        else if (typeof(T) == typeof(CustomWindow)) customWindow.Open();
        else if (typeof(T) == typeof(RankWindow)) rankWindow.Open();
        else if (typeof(T) == typeof(CharacterWindow)) characterWindow.Open();
        else if (typeof(T) == typeof(TrainingWindow)) trainingWindow.Open();
        else if (typeof(T) == typeof(SettingWindow)) settingWindow.Open();
        else if (typeof(T) == typeof(MatchTimeWindow)) matchTimeWindow.Open();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>() // UIMgr.Close<> 세분화
    {
        if (typeof(T) == typeof(MatchWindow)) matchWindow.Close();
        else if (typeof(T) == typeof(CustomWindow)) customWindow.Close();
        else if (typeof(T) == typeof(RankWindow)) rankWindow.Close();
        else if (typeof(T) == typeof(CharacterWindow)) characterWindow.Close();
        else if (typeof(T) == typeof(TrainingWindow)) trainingWindow.Close();
        else if (typeof(T) == typeof(SettingWindow)) settingWindow.Close();
        else if (typeof(T) == typeof(MatchTimeWindow)) matchTimeWindow.Close();
        else Debug.Log("범위 벗어남");
    }

    public void OnWindowButton(string btnType) // Windows setActive(True) When Button Click 
    {
        if (btnType == null)
            return;

        switch (btnType) 
        {
            case "Match":
                Managers.UI.OpenUI<MatchWindow>();
                break;
            case "Custom":
                Managers.UI.OpenUI<CustomWindow>();
                break;
            case "Rank":
                Managers.UI.OpenUI<RankWindow>();
                break;
            case "Character":
                Managers.UI.OpenUI<CharacterWindow>();
                break;
            case "Training":
                Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Main); // 테스트용 지울 예정
                // Managers.UI.OpenUI<TrainingWindow>();
                break;
            case "Setting":
                Managers.UI.OpenUI<SettingWindow>();
                break;
            default:
                Debug.Log("범위 벗어남");
                break;
        }
    }

    public void OffWindowButton(string btnType) // Windows SetActive(false) when CloseBtn Click
    {
        if (btnType == null)
            return;

        switch (btnType)
        {
            case "Match":
                Managers.UI.CloseUI<MatchWindow>();
                break;
            case "Custom":
                Managers.UI.CloseUI<CustomWindow>();
                break;
            case "Rank":
                Managers.UI.CloseUI<RankWindow>();
                break;
            case "Character":
                Managers.UI.CloseUI<CharacterWindow>();
                break;
            case "Training":
                Managers.UI.CloseUI<TrainingWindow>();
                break;
            case "Setting":
                Managers.UI.CloseUI<SettingWindow>();
                break;
            default:
                Debug.Log("범위 벗어남");
                break;
        }
    }

    public void SelectCharacter(int charType) // 캐릭터 선택 후 매칭중 창 활성 
    {
        matchTimeWindow.charType = (ENUM_CHARACTER_TYPE)charType;
        Managers.UI.OpenUI<MatchTimeWindow>();
        Managers.UI.CloseUI<MatchWindow>();
    }
}
