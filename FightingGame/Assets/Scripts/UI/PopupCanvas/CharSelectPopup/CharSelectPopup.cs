using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class CharSelectPopup : PopupUI
{
    [SerializeField] Image[] characterImages;
    [SerializeField] RectTransform selectionEffectRectTr;
    [SerializeField] Text charDescriptionText;

    Action<ENUM_CHARACTER_TYPE> onSelectionCharacter;

    ENUM_CHARACTER_TYPE selectedCharType = ENUM_CHARACTER_TYPE.Default;

    public void Open(Action<ENUM_CHARACTER_TYPE> _onSelectionCharacter)
    {
        if (_onSelectionCharacter == null)
        {
            Debug.LogError("SelectionCharacterCallBack is Null!");
            return;
        }

        onSelectionCharacter = _onSelectionCharacter;
        selectedCharType = ENUM_CHARACTER_TYPE.Default;
        charDescriptionText.text = "캐릭터를 선택해주세요.";

        if (selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void OnClick_CharacterSelectImage(int _charTypeNum)
    {
        selectedCharType = (ENUM_CHARACTER_TYPE)_charTypeNum;

        RectTransform rectTr = characterImages[_charTypeNum - 1].GetComponent<RectTransform>();
        selectionEffectRectTr.position = rectTr.position;

        charDescriptionText.text = Managers.Data.Get_CharExplanationDict(selectedCharType);

        if (!selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(true);
    }

    public void OnClick_SelectCompletion()
    {
        if (selectedCharType == ENUM_CHARACTER_TYPE.Default || selectedCharType == ENUM_CHARACTER_TYPE.Max)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }

        onSelectionCharacter?.Invoke(selectedCharType);
        OnClick_Exit();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        onSelectionCharacter = null;
        selectedCharType = ENUM_CHARACTER_TYPE.Default;
        gameObject.SetActive(false);
    }
}
