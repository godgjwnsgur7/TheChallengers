using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : UIElement
{
    [SerializeField] GameObject setBtn;
    [SerializeField] TopPanel topPanel;
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

    public void PushKey(UpdatableUI UpdateUI)
    {
        if (isOpen)
        {
            /*bottomPanel.setSlider(go);
            Managers.UI.OpenUI<BottomPanel>();*/

            bottomPanel.setSlider(UpdateUI);
            Managers.UI.OpenUI<BottomPanel>();
        }
        else
        {
            Debug.Log("ㅎㅎ");
        }
    }
}
