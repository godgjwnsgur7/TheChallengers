using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SettingPanel : UIElement
{
    [SerializeField] BottomPanel bottomPanel;
    [SerializeField] KeyPanelAreaEdit keyPanelArea;

    public bool isUpdate = false;
    public int unEditCount = 0;

    public override void Open(UIParam param = null)
    {
        isUpdate = true;
        keyPanelArea.SetUpdateComponentAll();
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

    private void Update()
    {
        if (unEditCount > 0)
            bottomPanel.isUpdatable = false;
        else
            bottomPanel.isUpdatable = true;
    }
}
