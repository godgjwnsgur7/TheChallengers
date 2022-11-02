using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class CharSelectPopup : PopupUI
{
    Action<ENUM_CHARACTER_TYPE> charTypeCallBack = null;

    public void Open(Action<ENUM_CHARACTER_TYPE> _charTypeCallBack)
    {
        if(_charTypeCallBack == null)
        {
            Debug.LogError("charCallBack is Null!");
            return;
        }

        charTypeCallBack = _charTypeCallBack;
        this.gameObject.SetActive(true);
    }

    public void OnClick_Char(string charTypeName)
    {
        ENUM_CHARACTER_TYPE charType = (ENUM_CHARACTER_TYPE)Enum.Parse(typeof(ENUM_CHARACTER_TYPE), charTypeName);

        if (charType == ENUM_CHARACTER_TYPE.Default || charType == ENUM_CHARACTER_TYPE.Max)
        {
            Debug.Log($"{charTypeName} 이 charType에 없습니다.");
            return;
        }

        charTypeCallBack(charType);
        Close();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        charTypeCallBack = null;
    }
}
