using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        keyPanelArea.SliderReset();
        isUpdate = false;
        base.Close();
    }

    public void PushKey(UpdatableUI updateUI)
    {
        if (isOpen)
        {
            bottomPanel.setSlider(updateUI);
            Managers.UI.OpenUI<BottomPanel>();
        }
        else
        {
            keyPanelArea.PushKey(updateUI);
        }
    }
}
