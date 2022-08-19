using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : UIElement
{
    [SerializeField] GameObject setBtn;
    [SerializeField] TopPanel topPanel;
    [SerializeField] BottomPanel bottomPanel;

    public override void Open(UIParam param = null)
    {
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
            bottomPanel.gameObject.SetActive(true);
            bottomPanel.setSlider(go);
        }
        else
        {
            Debug.Log("ㅎㅎ");
        }

    }
}
