using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class CharacterSelectElementUI : MonoBehaviour
{
    [SerializeField] Image charImage;
    [SerializeField] Text charNameText;

    Action<ENUM_CHARACTER_TYPE> selectCharCallBack;
    ENUM_CHARACTER_TYPE characterType;

    public void Init(Action<ENUM_CHARACTER_TYPE> _selectCharCallBack, ENUM_CHARACTER_TYPE _characterType)
    {
        selectCharCallBack = _selectCharCallBack;

       charNameText.text = Managers.Data.Get_CharNameDict(_characterType);
    }

    public void OnClick_CharElementUI()
    {
        selectCharCallBack(characterType);
    }
}
