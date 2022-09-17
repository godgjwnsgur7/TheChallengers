using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SettingPanel : UIElement
{
    [SerializeField] BottomPanel bottomPanel;
    [SerializeField] KeyPanelArea keyPanelArea;

    public bool isUpdate = false;

    public override void Open(UIParam param = null)
    {
        isUpdate = true;
        base.Open(param);
    }

    public override void Close()
    {
        keyPanelArea.SliderResetAll();
        isUpdate = false;
        keyPanelArea.RemoveUpdateComponentAll();
        base.Close();
    }

    public void PushKey(UpdatableUI updateUI)
    {
        if (isOpen)
        {
            bottomPanel.setSlider(updateUI);
            Managers.UI.OpenUI<BottomPanel>();
        }
    }
}
