using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBtn : ButtonUI
{
    [SerializeField] MatchWindow matchWindow;
    [SerializeField] RankWindow rankWindow;
    [SerializeField] CharacterWindow characterWindow;
    [SerializeField] SettingWindow settingWindow;

    public override void OffWindowButton()
    {
        if (matchWindow.gameObject.activeSelf)
            Managers.UI.CloseUI<MatchWindow>();
        else if (rankWindow.gameObject.activeSelf)
            Managers.UI.CloseUI<RankWindow>();
        else if (characterWindow.gameObject.activeSelf)
            Managers.UI.CloseUI<CharacterWindow>();
        else if (settingWindow.gameObject.activeSelf)
            Managers.UI.CloseUI<SettingWindow>();

        base.OffWindowButton();
    }
}
