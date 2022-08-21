using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : UIElement
{
    [SerializeField] GameObject setBtn;
    [SerializeField] TopPanel topPanel;
    [SerializeField] BottomPanel bottomPanel;
    [SerializeField] KeyPanelArea keyPanelArea;

    public override void Open(UIParam param = null)
    {
        keyPanelArea.SliderReset();
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void PushKey(GameObject go)
    {
        if (isOpen)
        {
            bottomPanel.setSlider(go);
            Managers.UI.OpenUI<BottomPanel>();
        }
        else
        {
            Debug.Log("ㅎㅎ");
        }
    }
}
