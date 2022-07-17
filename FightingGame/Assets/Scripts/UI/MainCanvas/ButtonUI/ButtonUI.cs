using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : UIElement
{
    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public virtual void OnWindowButton() 
    {
        Managers.UI.OpenUI<WindowUI>();
    }

    public virtual void OffWindowButton()
    {
        Managers.UI.CloseUI<WindowUI>();
    }
}
