using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class CharacterSelectArea : MonoBehaviour
{
    Action<ENUM_CHARACTER_TYPE> selectionCharacterCallBack;

    public void Init(Action<ENUM_CHARACTER_TYPE> _selectionCharacterCallBack)
    {
        selectionCharacterCallBack = _selectionCharacterCallBack;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_CharacterSelectImage(int _charTypeNum)
    {
        selectionCharacterCallBack((ENUM_CHARACTER_TYPE)_charTypeNum);
    }
}
