using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : UIElement
{
    [SerializeField] SettingPanel settingPanel;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void OnClick_ActivatingSettingPanel()
    {
        settingPanel.Open();
    }
}
