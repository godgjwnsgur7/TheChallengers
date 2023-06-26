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

    ENUM_CHARACTER_TYPE charType = ENUM_CHARACTER_TYPE.Default;

    public void Init(Action<ENUM_CHARACTER_TYPE> _selectionCharacterCallBack)
    {
        charType = ENUM_CHARACTER_TYPE.Default;

        selectionCharacterCallBack = _selectionCharacterCallBack;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        selectionEffectRectTr.gameObject.SetActive(false);
    }

    public void OnClick_CharacterSelectImage(int _charTypeNum)
    {
        if (charType == (ENUM_CHARACTER_TYPE)_charTypeNum)
            return;

        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Cilck_Heavy2);
        
        RectTransform rectTr = characterImages[_charTypeNum - 1].GetComponent<RectTransform>();
        selectionEffectRectTr.position = rectTr.position;

        if (!selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(true);

        if (selectionCharacterCallBack == null)
            Debug.LogError("selectionCharacterCallBack is Null");
        else
            selectionCharacterCallBack((ENUM_CHARACTER_TYPE)_charTypeNum);
    }
}
