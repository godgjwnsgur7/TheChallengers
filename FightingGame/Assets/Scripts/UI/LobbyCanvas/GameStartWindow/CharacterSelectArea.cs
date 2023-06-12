using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CharacterSelectArea : MonoBehaviour
{
    [SerializeField] Image[] characterImages;
    [SerializeField] RectTransform selectionEffectRectTr;

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
        RectTransform rectTr = characterImages[_charTypeNum - 1].GetComponent<RectTransform>();
        selectionEffectRectTr.position = rectTr.position;

        if (!selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(true);

        selectionCharacterCallBack((ENUM_CHARACTER_TYPE)_charTypeNum);
    }
}
