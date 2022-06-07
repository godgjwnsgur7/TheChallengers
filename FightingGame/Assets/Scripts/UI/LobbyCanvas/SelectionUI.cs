using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SelectionUI : UIElement
{
    public ENUM_CHARACTER_TYPE charType;

    public override void Open(UIParam param = null)
    {

    }

    public override void Close()
    {

    }

    public void OnCilckCharType(int intType)
    {
        charType = (ENUM_CHARACTER_TYPE)intType;
    }
    
}
