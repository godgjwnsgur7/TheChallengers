using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class CharSelectPopup : PopupUI
{

    Action<ENUM_CHARACTER_TYPE> charTypeCallBack = null;
    ENUM_CHARACTER_TYPE selectedCharType = ENUM_CHARACTER_TYPE.Default;

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
        selectedCharType = (ENUM_CHARACTER_TYPE)Enum.Parse(typeof(ENUM_CHARACTER_TYPE), charTypeName);
    

    }

    public void OnClick_Select()
    {
        if (selectedCharType == ENUM_CHARACTER_TYPE.Default || selectedCharType == ENUM_CHARACTER_TYPE.Max)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }

        charTypeCallBack(selectedCharType);
        OnClick_Exit();
    }

    public void OnClick_Exit()
    {
        this.gameObject.SetActive(false);

        charTypeCallBack = null;
        selectedCharType = ENUM_CHARACTER_TYPE.Default;
    }
}
