using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : UIElement
{
    [SerializeField] Text panelOpenBtnText;
    [SerializeField] SettingPanel settingPanel;
    [SerializeField] InputKeyManagement inputKeyManagement;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
        SetPanelOpenButtonText("닫기");
    }

    public override void Close()
    {
        base.Close();
        SetPanelOpenButtonText("설정");
    }

    public void OnClick_OpenSettingPanel()
    {
        inputKeyManagement.Init();
        this.Close();
        settingPanel.Open();

    }

    public void SetPanelOpenButtonText(string text)
    {
        panelOpenBtnText.text = text;
    }
}
